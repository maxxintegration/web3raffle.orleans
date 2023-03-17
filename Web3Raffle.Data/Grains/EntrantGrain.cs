using Web3raffle.Abstractions;
using Web3raffle.Models.Enums;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Data;
using Web3raffle.Shared;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class EntrantGrain : Grain, IEntrantGrain
{

	private async Task SetEntrantSequenceAsync(Web3RaffleEntrantModel entrant)
	{
		var nextNumber = await this.GrainFactory.GetGrain<IAutomicNumberGrain>(this.GetPrimaryKey()).GetNext();

		entrant.EntrantSequence = nextNumber;
		entrant.Id = Guid.NewGuid().ToString();
	}

	private byte[] DownloadRaffleEntrants(IList<Web3RaffleEntrantModel> raffleEntrants)
	{
		var csv = new StringBuilder();

		csv.AppendLine($"WalletAddress, DisplayName");
		foreach (var raffleEntry in raffleEntrants)
		{
			csv.AppendLine($"\"{raffleEntry.WalletAddress}\",\"{raffleEntry.DisplayName}\"");
		}

		return Encoding.UTF8.GetBytes(csv.ToString());
	}

	public async Task<int> GetEntrantCountAsync(string raffleId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var queryParams = new QueryModel();

		queryParams.AppendFilter($"raffleId = '{raffleId}'");

		return await grain.Count(queryParams, ct);
	}

	public async Task<int> GetEntrantCountByAddressAsync(string raffleId, string walletAddress, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var queryParams = new QueryModel();

		queryParams.AppendFilter($"raffleId = '{raffleId}' and walletAddress = '{walletAddress}'");

		return await grain.Count(queryParams, ct);
	}

	public async Task<List<Web3RaffleEntrantModel>> GetWinningEntrantsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		queryParams.AppendFilter($"raffleId = '{raffleId}' and isWinner = true");

		return await grain.Read(queryParams, ct);
	}

	public async Task<int> GetWinningEntrantsCountAsync(string raffleId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var queryParams = new QueryModel();

		queryParams.AppendFilter($"raffleId = '{raffleId}' and isWinner = true");

		return await grain.Count(queryParams, ct);
	}

	public async Task<List<Web3RaffleEntrantModel>> GetEntrantsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		queryParams.AppendFilter($"raffleId = '{raffleId}'");

		queryParams.Filter = $"{queryParams.Filter} {(string.IsNullOrEmpty(queryParams.Filter) ? "" : " and ")} deleted = false";

		return await grain.Read(queryParams, ct);
	}

	public async Task<int> GetEntrantCountAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		queryParams.AppendFilter($"raffleId = '{raffleId}'");

		queryParams.Filter = $"{queryParams.Filter} {(string.IsNullOrEmpty(queryParams.Filter) ? "" : " and ")} deleted = false";

		return await grain.Count(queryParams, ct);
	}

	public async Task<Web3RaffleEntrantModel> GetEntrantAsync(string entrantId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		return await grain.Read(entrantId, ct);
	}


	public async Task CreateEntrantAsync(List<Web3RaffleEntrantModel> entrants, GrainCancellationToken ct)
	{
		if (entrants!.Count == 0)
			return;

		string raffleId = entrants[0].RaffleId;

		//var userGrain = this.GrainFactory.GetGrain<IUserGrain>(this.GetPrimaryKey());
		var raffleGrain = this.GrainFactory.GetGrain<IRaffleGrain>(this.GetPrimaryKey());
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var addressInfos = new List<Web3RaffleEntrantModel>();

		// VALIDATE RAFFLE BEFORE ENTER
		var raffle = await raffleGrain.ValidateRaffleBeforeEnterAsync(raffleId, entrants, entrants[0].RafflePassword, ct);

		//var raffle = await raffleGrain.GetRaffleAsync(raffleId, false, ct);

		List<Web3RaffleWhitelistModel> whitelist = null!;

		if (raffle.EnableWhiteList)
		{
			var whiteListGrain = this.GrainFactory.GetGrain<IWhitelistGrain>(this.GetPrimaryKey());
			whitelist = await whiteListGrain.GetWhitelistByWalletAddressAsync(entrants.Select(x => x.WalletAddress.ToLower()).ToList(), raffleId, ct);
		}

		// ADD ENTRANT TO RAFFLE
		foreach (var entrant in entrants)
		{

			// POPULATE DISPLAY NAME AND AVATAR
			//if (addressInfos.Where(x => x.WalletAddress != entrant.WalletAddress).Count() == 0)
			//{
			//	var externalProfile = await userGrain.GetPublicContactByAddressOrProfileId(entrant.WalletAddress, ct);

			//	if (externalProfile != null)
			//	{
			//		addressInfos.Add(new Web3RaffleEntrantModel
			//		{
			//			WalletAddress = entrant.WalletAddress,
			//			DisplayName = externalProfile.DisplayName,
			//			AvatarUrl = externalProfile.AvatarUrl
			//		});
			//	}
			//}

			// SET DISPLAY NAME/AVATAR URL BACK ENTRANT.....
			var addressInfo = addressInfos.Where(x => x.WalletAddress == entrant.WalletAddress).FirstOrDefault();
			if (addressInfo != null)
			{
				entrant.DisplayName = addressInfo.DisplayName;
				entrant.AvatarUrl = addressInfo.AvatarUrl;
			}

			// INSERT ENTRANT INTO DB
			if (raffle!.EnableWhiteList)
			{
				int limitCount = whitelist!.Where(x => x.WalletAddress.ToLower() == entrant.WalletAddress.ToLower()).Select(x => x.LimitCount).FirstOrDefault();
				int entrantCount = await this.GetEntrantCountByAddressAsync(raffle.Id, entrant.WalletAddress, ct);

				limitCount = limitCount - entrantCount;
				for (int i = 0; i < limitCount; i++)
				{
					await this.SetEntrantSequenceAsync(entrant);
					await grain.Write(entrant, ct);
				}

			}
			else
			{
				await this.SetEntrantSequenceAsync(entrant);
				await grain.Write(entrant, ct);
			}

			// ADD EVENT LOG
			var eventLog = new Web3RaffleEventLogModel()
			{
				RaffleId = raffleId,
				LogType = EventLogType.Entrant,
				LogMessage = $"Raffle entrant is created"
			};

			await this.GrainFactory.ProcessEvent<ICreateWeb3RaffleEventLogEvent, Web3RaffleEventLogModel>("10000000", eventLog, ct);
		}

		// UPDATE NUMBER OF RAFFLE ENTRANTS
		raffle.NumberOfEntrants = await this.GetEntrantCountAsync(raffleId, ct);
		await raffleGrain.UpdateRaffleAsync(raffle, ct);
	}

	public async Task DeleteEntrantAsync(string entrantId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		await grain.Delete(entrantId.ToLower(), ct);
	}

	public async Task DeleteEntrantByWalletAddressAsync(string raffleId, string walletAddress, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var queryParams = new QueryModel();
		queryParams.AppendFilter($"raffleId = '{raffleId}' and walletAddress = '{walletAddress}'");

		var entrants = await grain.Read(queryParams, ct);
		if (entrants != null)
		{
			foreach (var entrant in entrants)
				await grain.Delete(entrant.Id, ct);
		}

	}

	public async Task DeleteEntrantsAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryParams = new QueryModel() { Top = 1000000 };
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEntrantModel>>(this.GetPrimaryKey());

		var entrants = await this.GetEntrantsAsync(raffleId, queryParams, ct);

		foreach (var entrant in entrants)
		{
			await grain.Delete(entrant.Id, ct);
		}
	}

	public async Task<byte[]> DownloadEntrantsAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryParams = new QueryModel() { Top = 1000000 };
		var raffleEntrants = await this.GetEntrantsAsync(raffleId, queryParams, ct);
		return this.DownloadRaffleEntrants(raffleEntrants);
	}

	public async Task<byte[]> DownloadWinnersAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryParams = new QueryModel() { Top = 1000000 };
		var raffleEntrants = await this.GetWinningEntrantsAsync(raffleId, queryParams, ct);
		return this.DownloadRaffleEntrants(raffleEntrants);
	}
}