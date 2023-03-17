using Web3raffle.Models.Data;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IBlacklistGrain : IGrainWithGuidKey
	{
		Task<Web3RaffleBlacklistModel?> GetBlacklistAsync(string raffleId, string walletAddress, GrainCancellationToken ct);

		Task<List<string>> GetEntrantBlacklistAddressOnlyAsync(List<string> entrantWalletAddresses, string raffleId, GrainCancellationToken ct);

		Task<List<Web3RaffleBlacklistModel>> GetBlacklistAsync(string raffleId, GrainCancellationToken ct);

		Task CreateBlacklistAsync(List<Web3RaffleBlacklistModel> listModel, GrainCancellationToken ct);

		Task DeleteBlacklistAsync(List<Web3RaffleBlacklistModel> listModel, GrainCancellationToken ct);

		Task DeleteBlacklistAsync(string raffleId, GrainCancellationToken ct);
	}
}