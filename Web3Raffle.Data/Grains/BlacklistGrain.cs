using Web3raffle.Models.Requests;
using Web3raffle.Models.Data;
using Web3raffle.Shared;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class BlacklistGrain : Grain, IBlacklistGrain
{
	public async Task<List<Web3RaffleBlacklistModel>> GetBlacklistAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryFilter = new RaffleQueryModel();

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleBlacklistModel>>(this.GetPrimaryKey());

		queryFilter.AppendFilter($"raffleId eq '{raffleId}'");
		queryFilter.Top = 100000;

		return await container.Read(queryFilter, ct) ?? new List<Web3RaffleBlacklistModel>();
	}

	public async Task<Web3RaffleBlacklistModel?> GetBlacklistAsync(string raffleId, string walletAddress, GrainCancellationToken ct)
	{
		var queryFilter = new RaffleQueryModel();

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleBlacklistModel>>(this.GetPrimaryKey());

		queryFilter.AppendFilter($"raffleId eq '{raffleId}' and walletAddress eq '{walletAddress}'");

		var data = await container.Read(queryFilter, ct);

		return data?.Count > 0 ? data[0] : null;
	}

	public async Task CreateBlacklistAsync(List<Web3RaffleBlacklistModel> listModel, GrainCancellationToken ct)
	{
		if (listModel!.Count == 0)
			return;

		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleBlacklistModel>>(this.GetPrimaryKey());

		var currentBlacklist = await this.GetBlacklistAsync(listModel[0].RaffleId, ct);

		foreach (var item in listModel)
		{
			var exits = currentBlacklist.Where(x => x.WalletAddress.ToLower() == item.WalletAddress.ToLower()).FirstOrDefault();

			if (exits is null)
			{
				await container.Write(item, ct);
				currentBlacklist.Add(item);
			}
		}
	}

	public async Task DeleteBlacklistAsync(string raffleId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleBlacklistModel>>(this.GetPrimaryKey());

		var blacklist = await this.GetBlacklistAsync(raffleId, ct);

		foreach (var item in blacklist)
		{
			await grain.Delete(item.Id, ct);
		}
	}

	public async Task DeleteBlacklistAsync(List<Web3RaffleBlacklistModel> listModel, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleBlacklistModel>>(this.GetPrimaryKey());

		foreach (var item in listModel)
		{
			await container.Delete(item.Id, ct);
		}
	}

	public async Task<List<string>> GetEntrantBlacklistAddressOnlyAsync(List<string> entrantWalletAddresses, string raffleId, GrainCancellationToken ct)
	{
		var blacklist = await this.GetBlacklistAsync(raffleId, ct);

		var entrantBlackList = blacklist.Where(x => entrantWalletAddresses.Contains(x.WalletAddress.ToLower())).Select(x => x.WalletAddress).ToList();

		return entrantBlackList;
	}
}