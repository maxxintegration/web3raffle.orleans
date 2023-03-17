using Web3raffle.Models.Requests;
using Web3raffle.Models.Data;
using Web3raffle.Shared;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class WhitelistGrain : Grain, IWhitelistGrain
{
	public async Task<List<Web3RaffleWhitelistModel>> GetWhitelistAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryFilter = new RaffleQueryModel();

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleWhitelistModel>>(this.GetPrimaryKey());

		queryFilter.AppendFilter($"raffleId eq '{raffleId}'");
		queryFilter.Top = 100000;

		return await container.Read(queryFilter, ct) ?? new List<Web3RaffleWhitelistModel>();
	}

	public async Task<Web3RaffleWhitelistModel?> GetWhitelistAsync(string raffleId, string walletAddress, GrainCancellationToken ct)
	{
		var queryFilter = new RaffleQueryModel();

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleWhitelistModel>>(this.GetPrimaryKey());

		queryFilter.AppendFilter($"raffleId eq '{raffleId}' and walletAddress eq '{walletAddress}'");

		var data = await container.Read(queryFilter, ct);

		return data?.Count > 0 ? data[0] : null;
	}

	public async Task CreateWhitelistAsync(List<Web3RaffleWhitelistModel> listModel, GrainCancellationToken ct)
	{
		if (listModel!.Count == 0)
			return;

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleWhitelistModel>>(this.GetPrimaryKey());

		var currentWhitelist = await this.GetWhitelistAsync(listModel[0].RaffleId, ct);

		foreach (var item in listModel)
		{
			var exits = currentWhitelist.Where(x => x.WalletAddress.ToLower() == item.WalletAddress.ToLower()).FirstOrDefault();

			if (exits is null)
			{
				await container.Write(item, ct);
				currentWhitelist.Add(item);
			}
		}
	}

	public async Task DeleteWhitelistAsync(string raffleId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleWhitelistModel>>(this.GetPrimaryKey());

		var whitelist = await this.GetWhitelistAsync(raffleId, ct);

		foreach (var item in whitelist)
		{
			await grain.Delete(item.Id, ct);
		}
	}

	public async Task DeleteWhitelistAsync(List<Web3RaffleWhitelistModel> listModel, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleWhitelistModel>>(this.GetPrimaryKey());

		foreach (var item in listModel)
		{
			await container.Delete(item.Id, ct);
		}
	}

	public async Task<List<Web3RaffleWhitelistModel>> GetWhitelistByWalletAddressAsync(List<string> entrantWalletAddresses, string raffleId, GrainCancellationToken ct)
	{
		var whitelist = await this.GetWhitelistAsync(raffleId, ct);

		var entrantWhiteList = whitelist.Where(x => entrantWalletAddresses.Contains(x.WalletAddress.ToLower())).ToList();

		return entrantWhiteList;
	}

	public async Task<(int, int)> GetWhitelistEntrantCountAsync(List<string> walletAddress, string raffleId, GrainCancellationToken ct)
	{
		int maxWhitelistEntrant = 0;
		int numberOfEntering = 0;
		var whiteList = await this.GetWhitelistAsync(raffleId, ct);

		foreach (var item in whiteList)
		{
			maxWhitelistEntrant += item.LimitCount;

			// COUNT NUMBER OF ENTERING
			if (walletAddress.Contains(item.WalletAddress.ToLower()))
				numberOfEntering += item.LimitCount;
		}

		return (maxWhitelistEntrant, numberOfEntering);
	}
}