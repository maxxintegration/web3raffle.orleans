using Web3raffle.Models.Data;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IWhitelistGrain : IGrainWithGuidKey
	{
		Task<List<Web3RaffleWhitelistModel>> GetWhitelistAsync(string raffleId, GrainCancellationToken ct);

		Task<Web3RaffleWhitelistModel?> GetWhitelistAsync(string raffleId, string walletAddress, GrainCancellationToken ct);

		Task<List<Web3RaffleWhitelistModel>> GetWhitelistByWalletAddressAsync(List<string> entrantWalletAddresses, string raffleId, GrainCancellationToken ct);

		Task<(int, int)> GetWhitelistEntrantCountAsync(List<string> walletAddress, string raffleId, GrainCancellationToken ct);

		Task CreateWhitelistAsync(List<Web3RaffleWhitelistModel> listModel, GrainCancellationToken ct);

		Task DeleteWhitelistAsync(List<Web3RaffleWhitelistModel> listModel, GrainCancellationToken ct);

		Task DeleteWhitelistAsync(string raffleId, GrainCancellationToken ct);
	}
}