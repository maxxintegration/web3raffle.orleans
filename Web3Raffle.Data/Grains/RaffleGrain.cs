using System.Security.Cryptography;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;
using Web3raffle.Abstractions;
using Web3raffle.Models.Responses;
using Web3raffle.Models.Enums;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Data;
using Web3raffle.Shared;

//using VeeFriends.Utilities.Helpers;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class RaffleGrain : Grain, IRaffleGrain
{
	private string GetSlug(string name, DateTimeOffset startDate)
	{
		return $"{name.ToLower().Trim().ToUrlSlug()}_{string.Format("{0:yyyy-MM-dd}", startDate)}";
	}

	private async Task UpdateStatusAsync(string raffleId, RaffleStatus status, GrainCancellationToken ct)
	{
		var raffle = await this.GetRaffleAsync(raffleId, false, ct);
		await this.UpdateStatusAsync(raffle, status, ct);

		if (raffle != null)
		{
			string message = $"Raffle is {(status == RaffleStatus.InProgress ? "in progress" : status == RaffleStatus.End ? "ended" : status.ToString())}";
			this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(raffle.Id, EventLogType.Raffle, message, ct);
		}

	}

	private async Task UpdateStatusAsync(Web3RaffleModel raffle, RaffleStatus status, GrainCancellationToken ct)
	{

		raffle.Status = status;
		if (status == RaffleStatus.End)
			raffle.EndDate = DateTimeOffset.UtcNow;
		else if (status == RaffleStatus.InProgress)
		{
			if (raffle.EndDate <= raffle.StartDate)
				raffle.EndDate = null;

			raffle.StartDate = DateTimeOffset.UtcNow;
		}

		await this.UpdateRaffleAsync(raffle, ct);
	}

	private async Task AutoUpdateStatusAsync(Web3RaffleModel raffle, GrainCancellationToken ct)
	{

		bool isUpdate = false;

		// AUTO UPDATE STATUS TO IN-PROGRESS
		if (raffle.Status == RaffleStatus.Initial && raffle.StartDate <= DateTimeOffset.UtcNow)
		{
			raffle.Status = RaffleStatus.InProgress;
			await this.UpdateRaffleAsync(raffle, ct);
			isUpdate = true;
		}
		// AUTO UPDATE STATUS TO END
		else if (raffle.Status == RaffleStatus.InProgress && raffle.EndDate <= DateTimeOffset.UtcNow)
		{
			raffle.Status = RaffleStatus.End;
			await this.UpdateRaffleAsync(raffle, ct);
			isUpdate = true;
		}

		if (isUpdate)
		{
			string message = $"Raffle is {(raffle.Status == RaffleStatus.InProgress ? "in progress" : raffle.Status == RaffleStatus.End ? "ended" : raffle.Status.ToString())}";
			this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(raffle.Id, EventLogType.Raffle, message, ct);
		}
	}

	private string CalculateMD5Hash(string input)
	{
		// Calculate MD5 hash from input
		using var md5 = MD5.Create();

		byte[] inputBytes = Encoding.ASCII.GetBytes(input);
		byte[] hash = md5.ComputeHash(inputBytes);

		// Convert byte array to hex string
		var sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("X2"));
		}

		return sb.ToString();
	}

	public async Task<List<Web3RaffleResponseModel>> GetRafflesAsync(QueryModel queryParams, bool maskPassword, GrainCancellationToken ct)
	{
		var projectGrain = this.GrainFactory.GetGrain<IProjectGrain>(this.GetPrimaryKey());
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());

		var responseRaffles = new List<Web3RaffleResponseModel>();

		queryParams.AppendFilter("deleted = false");

		List<Web3RaffleModel> raffles = await grain.Read(queryParams, ct);
		if (raffles != null && raffles.Count > 0)
		{

			PropertyCopier<Web3RaffleModel, Web3RaffleResponseModel>.CopyCollection(raffles, responseRaffles);
			var projectIds = raffles.Select(x => x.ProjectId).Distinct().ToList();

			// GET PROJECTS
			var projectParams = new QueryModel();
			projectParams.AppendFilter($"ARRAY_CONTAINS(['{string.Join("','", projectIds)}'], T1.id)");
			var projects = await projectGrain.GetProjectsAsync(projectParams, ct);

			// UPDATE PROJECT DEATAILS IF NOT IN RAFFLE
			responseRaffles = responseRaffles.Join(projects, r => r.ProjectId, p => p.Id, (r, p) =>
			{
				r.ProjectName = p.Name;
				r.ProjectDescription = p.Description;
				r.ExternalURL = string.IsNullOrEmpty(r.ExternalURL) ? p.ExternalURL : r.ExternalURL;
				r.RaffleLogoURL = string.IsNullOrEmpty(r.RaffleLogoURL) ? p.RaffleLogoURL : r.RaffleLogoURL;
				r.TermsOfUseExternalUrl = string.IsNullOrEmpty(r.TermsOfUseExternalUrl) ? p.TermsOfUseExternalUrl : r.TermsOfUseExternalUrl;
				r.PrivacyPolicyExternalURL = string.IsNullOrEmpty(r.PrivacyPolicyExternalURL) ? p.PrivacyPolicyExternalURL : r.PrivacyPolicyExternalURL;

				// MASK PASSWORD
				if (maskPassword)
					r.Password = r.Password.MaskString(r.PasswordProtect);

				return r;
			}).ToList();

			// UPDATE STATUS IF RETRIEVE SINGLE RAFFLE
			if ((queryParams.Filter ?? "").ToLower().Contains("urlslug") && raffles.Count == 1 && (raffles[0].Status == RaffleStatus.Initial || raffles[0].Status == RaffleStatus.InProgress))
			{
				var raffle = raffles[0];
				await this.AutoUpdateStatusAsync(raffle, ct);

				responseRaffles[0].Status = raffle.Status;
			}

			// ASSUME USER SEE INDIVISUAL RAFFLE
			if (raffles.Count == 1)
			{
				this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(raffles[0].Id, EventLogType.Raffle, $"Raffle is viewed", ct);
			}

		}

		return responseRaffles;
	}

	public async Task<int> GetRaffleCountAsync(QueryModel queryParams, GrainCancellationToken ct)
	{
		queryParams.Filter = $"{queryParams.Filter} {(string.IsNullOrEmpty(queryParams.Filter) ? "" : " and ")} deleted = false";

		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());

		return await grain.Count(queryParams, ct);
	}

	// autoUpdateStatus SET TO TRUE WHEN INDIVIDUAL RAFFLE IS REQUESTED BY A ROUTE
	public async Task<Web3RaffleModel> GetRaffleAsync(string id, bool autoUpdateStatus, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());

		var raffle = await container.Read(id.ToLower(), ct);

		bool deleted = raffle?.Deleted ?? true;

		if (!deleted)
		{
			if (autoUpdateStatus)
				await this.AutoUpdateStatusAsync(raffle!, ct);

			if (raffle != null && autoUpdateStatus)
			{
				this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(raffle.Id, EventLogType.Raffle, $"Raffle is viewed", ct);
			}
		}
		return (raffle == null || deleted ? null : raffle)!;
	}

	public async Task CreateRaffleAsync(Web3RaffleModel model, string? connectionId, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());

		model.UrlSlug = this.GetSlug(model.Name, model.StartDate);

		await container.Write(model, ct);

		this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(model.Id, EventLogType.Raffle, $"Raffle is created", ct);
	}

	public async Task UpdateRaffleAsync(Web3RaffleModel model, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());

		var raffle = await this.GetRaffleAsync(model.Id, false, ct);

		if (raffle == null) return;

		if (!model.CustomRandomizerSeed || model.RandomizeSeed < 1)
			model.RandomizeSeed = new CryptoRandom().Next();

		model.UrlSlug = this.GetSlug(model.Name, model.StartDate);
		model.ProjectId = raffle.ProjectId;
		model.CreatedBy = raffle.CreatedBy;
		model.Tags = raffle.Tags;
		model.CreatedAt = raffle.CreatedAt;
		model.ModifiedAt = DateTimeOffset.UtcNow;
		model.Deleted = false;

		await container.Update(model, ct);

		this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey()).LogThis(model.Id, EventLogType.Raffle, $"Raffle is updated", ct);

	}

	public async Task DeleteRaffleAsync(string id, GrainCancellationToken ct)
	{
		var queryParams = new QueryModel() { Top = 1000000 };
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleModel>>(this.GetPrimaryKey());
		var entrantGrain = this.GrainFactory.GetGrain<IEntrantGrain>(this.GetPrimaryKey());
		var whitelistGrain = this.GrainFactory.GetGrain<IWhitelistGrain>(this.GetPrimaryKey());
		var blacklistGrain = this.GrainFactory.GetGrain<IBlacklistGrain>(this.GetPrimaryKey());
		var eventLogGrain = this.GrainFactory.GetGrain<IEventLogGrain>(this.GetPrimaryKey());

		var raffle = await grain.Read(id.ToLower(), ct);

		await entrantGrain.DeleteEntrantsAsync(id, ct);
		await whitelistGrain.DeleteWhitelistAsync(id, ct).ConfigureAwait(false);
		await blacklistGrain.DeleteBlacklistAsync(id, ct).ConfigureAwait(false);
		await eventLogGrain.DeleteEventLogsAsync(id, ct).ConfigureAwait(false);
		await grain.Delete(raffle.Id, ct).ConfigureAwait(false);
	}

	public async Task BeginRaffleAsync(string raffleId, GrainCancellationToken ct)
	{
		await this.UpdateStatusAsync(raffleId, RaffleStatus.InProgress, ct);
	}

	public async Task EndRaffleAsync(string raffleId, GrainCancellationToken ct)
	{
		await this.UpdateStatusAsync(raffleId, RaffleStatus.End, ct);
	}

	public async Task DrawRaffleAsync(string raffleId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());
		var entrantGrain = this.GrainFactory.GetGrain<IEntrantGrain>(this.GetPrimaryKey());
		var raffle = await this.GetRaffleAsync(raffleId, false, ct);

		bool isDrawDate = raffle != null && raffle.EndDate < DateTimeOffset.UtcNow;
		var queryParams = new QueryModel() { Top = 1000000 };

		if (!isDrawDate)
			throw new Web3RaffleException("Raffle is still on going");


		var entrants = await entrantGrain.GetEntrantsAsync(raffleId, queryParams, ct);
		var drawingEntrants = new List<Web3RaffleEntrantDrawingModel>();
		var winningEntrants = new List<Web3RaffleWinnerModel>();

		// CONVERT HEX TO NUMBER
		//int numValue = int.Parse("7b205395", System.Globalization.NumberStyles.HexNumber);

		//USER HAS A CHANCE TO WIN SAME MULTIPLE WALLET ADDRESSES
		if (raffle!.AllowDuplicateWinner)
		{
			drawingEntrants = entrants.Select(x => new Web3RaffleEntrantDrawingModel
			{
				WalletAddress = x.WalletAddress,
				HashBytes = this.CalculateMD5Hash((raffle.RandomizeSeed + x.EntrantSequence).ToString())
			}).ToList();
		}
		else
		{
			var addresses = entrants.Select(x => x.WalletAddress).Distinct().ToList();

			foreach (var address in addresses)
			{
				var entrant = entrants.Where(x => x.WalletAddress == address).FirstOrDefault();
				drawingEntrants.Add(new Web3RaffleEntrantDrawingModel
				{
					WalletAddress = entrant!.WalletAddress,
					HashBytes = this.CalculateMD5Hash((raffle.RandomizeSeed + entrant.EntrantSequence).ToString())
				});
			}
		}

		// ORDER BY HASHBYTES AND GET THE WINNERS BASED ON TOP WINING SELECTION NUMBER
		drawingEntrants = drawingEntrants.OrderBy(x => x.HashBytes).ToList();
		for (int i = 0; i < raffle.WinningSelectionCount; i++)
		{
			var entrant = entrants.Where(x => x.WalletAddress == drawingEntrants[i].WalletAddress && !x.IsWinner).FirstOrDefault();
			if (entrant != null)
			{
				entrant.IsWinner = true;
				await grain.Update(entrant, ct);

				// GET WINNING ENTRANTS
				if (!winningEntrants.Select(x => x.WalletAddress).Contains(entrant.WalletAddress))
				{
					winningEntrants.Add(new Web3RaffleWinnerModel
					{
						RaffleId = entrant.RaffleId,
						RaffleName = raffle.Name,
						WalletAddress = entrant.WalletAddress,
						DisplayName = entrant.DisplayName,
						Email = entrant.Email
					});

				}

			}
		}

		// UPDATE RAFFLE AND SET STATUS TO DRAWN
		raffle.RaffleDrawnDate = DateTimeOffset.UtcNow;
		raffle.Status = RaffleStatus.Drawn;
		await this.UpdateRaffleAsync(raffle, ct);

		// SEND EMAIL TO NOTIFY WINNING USERS
		await this.GrainFactory.ProcessEvent<ISendRaffleWinnerNotificationEvent, Web3RaffleWinnerModel>(null, winningEntrants, ct);

	}

	public async Task ValidateRaffleAsync(Web3RaffleModel raffle, bool newRaffle, GrainCancellationToken ct)
	{

		var projectGrain = this.GrainFactory.GetGrain<IProjectGrain>(this.GetPrimaryKey());
		var project = await projectGrain.GetProjectAsync(raffle.ProjectId, ct);

		if (project == null)
			throw new Web3RaffleException("Invalid project id.");

		if (raffle.StartDate > raffle.EndDate)
			throw new Web3RaffleException("Start date can not be greater than End date.");

		if (raffle.EnableMaxEntrant && raffle.WinningSelectionCount > raffle.MaxEntrant)
			throw new Web3RaffleException("WinningSelectionCount can not be greater than MaxEntrant.");

		if (newRaffle && raffle.DisablePublicEntrance)
		{
			if (string.IsNullOrEmpty(raffle.Password))
				throw new Web3RaffleException("Password is required for private raffle.");

			raffle.PasswordProtect = true;

		}
	}


	public async Task<Web3RaffleModel> ValidateRaffleBeforeEnterAsync(string raffleId, List<Web3RaffleEntrantModel> entrants, string? rafflePassword, GrainCancellationToken ct)
	{
		var entrantGrain = this.GrainFactory.GetGrain<IEntrantGrain>(this.GetPrimaryKey());
		var blackListGrain = this.GrainFactory.GetGrain<IBlacklistGrain>(this.GetPrimaryKey());
		var whiteListGrain = this.GrainFactory.GetGrain<IWhitelistGrain>(this.GetPrimaryKey());
		var entrantWalletAddresses = new List<string>();

		// CHECK FOR VALID ETHEREUM ADDRESS
		foreach (var item in entrants)
		{
			if (!Nethereum.Util.AddressUtil.Current.IsValidEthereumAddressHexFormat(item.WalletAddress))
				throw new Web3RaffleException($"{item.WalletAddress} is not a valid ethereum wallet address!");

			entrantWalletAddresses.Add(item.WalletAddress.ToLower());
		}

		var raffle = await this.GetRaffleAsync(raffleId, false, ct);
		if (raffle is null)
			throw new Web3RaffleException("Raffle does not exist!");
		else if (raffle.StartDate > DateTimeOffset.UtcNow)
			throw new Web3RaffleException("Raffle does not start yet!");
		else if (raffle.EndDate < DateTimeOffset.UtcNow)
			throw new Web3RaffleException("Raffle had ended!");
		else if (raffle.PasswordProtect && raffle.Password != rafflePassword)
			throw new Web3RaffleException("Password is required for this raffle!");
		//else if (raffle.DisablePublicEntrance)
		//	throw new Web3RaffleException("Only raffle creator can enter the entrant(s)!");

		// CHECK IF ADDRESSES ARE IN THE BLACK LIST.  IF ADDRESS IN THE BLACKLIST THEN THAT ADDRESS CAN NOT ENTER RAFFLE
		if (raffle!.EnableBlackList)
		{
			var blacklistCount = (await blackListGrain.GetEntrantBlacklistAddressOnlyAsync(entrantWalletAddresses, raffle.Id, ct)).Count;
			if (blacklistCount > 0)
				throw new Web3RaffleException($"Oops Sorry, invalid Wallet Address{(blacklistCount > 1 ? "es" : "")} (address{(blacklistCount > 1 ? "es" : "")} found in black list)!");
		}

		// ONLY ADDRESSES IN THE WHITE LIST CAN ENTER RAFFLE
		if (raffle!.EnableWhiteList)
		{
			// RETURN IF ENTRANT NOT IN WHITE LIST
			var whitelistCount = (await whiteListGrain.GetWhitelistByWalletAddressAsync(entrantWalletAddresses, raffle.Id, ct)).Count;
			if (whitelistCount == 0)
				throw new Web3RaffleException($"Oops sorry, Wallet Address{(whitelistCount > 1 ? "es" : "")} {(whitelistCount > 1 ? "are" : "is")} not in the white list!");

			//// RETURN IF REACH LIMIT OF WHITE LIST ALLOWANCE
			//(int maxWhitelistEntrant, int numberOfEntering) = await whiteListGrain.GetWhitelistEntrantCountAsync(entrantWalletAddresses, raffle.Id, ct);
			//if ((numberOfEntering + raffle.NumberOfEntrants) > maxWhitelistEntrant)
			//	errorMessage = $"Oops sorry, you have reached your entrant limit of {maxWhitelistEntrant}!";
		}
		else if (raffle.EnableMaxEntrant)
		{
			if (raffle.NumberOfEntrants >= raffle.MaxEntrant)
				throw new Web3RaffleException($"Exeeced maxinum number of entrants({raffle.MaxEntrant})!");
			else
			{
				// RETURN IF REACH LIMIT ALLOWANCE
				foreach (var address in entrantWalletAddresses)
				{
					int enteringCount = entrantWalletAddresses.Where(x => x == address).Count();
					int entrantCount = await entrantGrain.GetEntrantCountByAddressAsync(raffle.Id, address, ct);
					if (entrantCount + enteringCount > raffle.LimitCount)
						throw new Web3RaffleException($"Oops sorry, {(entrantWalletAddresses.Count > 1 ? $"Wallet Address{address} has reached its" : "you have reached your")} entrant limit of {raffle.LimitCount}!");
				}
			}
		}

		return raffle;
	}

	public async Task<byte[]> DownloadCalendarAsync(string id, GrainCancellationToken ct)
	{
		var raffle = await this.GetRaffleAsync(id, false, ct);

		if (raffle != null)
		{
			var now = DateTimeOffset.UtcNow;

			var later = now.AddHours(1);

			var calEvent = new CalendarEvent
			{
				Start = new CalDateTime(raffle.StartDate.DateTime),
				Summary = raffle.Name,
				Description = raffle.Description
			};

			if (raffle.EndDate.HasValue)
			{
				calEvent.End = new CalDateTime(raffle.EndDate.Value.DateTime);
			}

			var calendar = new Calendar();

			calendar.Events.Add(calEvent);

			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);

			return Encoding.UTF8.GetBytes(serializedCalendar);
		}
		else
		{
			throw new Exception("Raffle not found");
		}
	}
}