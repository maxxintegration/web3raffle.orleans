using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IEntrantGrain : IGrainWithGuidKey
	{
		Task<int> GetEntrantCountAsync(string raffleId, GrainCancellationToken ct);

		Task<int> GetEntrantCountByAddressAsync(string raffleId, string walletAddress, GrainCancellationToken ct);

		Task<List<Web3RaffleEntrantModel>> GetWinningEntrantsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct);

		Task<int> GetWinningEntrantsCountAsync(string raffleId, GrainCancellationToken ct);

		Task<List<Web3RaffleEntrantModel>> GetEntrantsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct);

		Task<int> GetEntrantCountAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct);

		Task<Web3RaffleEntrantModel> GetEntrantAsync(string entrantId, GrainCancellationToken ct);

		Task CreateEntrantAsync(List<Web3RaffleEntrantModel> listModel, GrainCancellationToken ct);

		Task DeleteEntrantAsync(string entrantId, GrainCancellationToken ct);

		Task DeleteEntrantsAsync(string raffleId, GrainCancellationToken ct);

		Task DeleteEntrantByWalletAddressAsync(string raffleId, string walletAddress, GrainCancellationToken ct);

		Task<byte[]> DownloadEntrantsAsync(string raffleId, GrainCancellationToken ct);

		Task<byte[]> DownloadWinnersAsync(string raffleId, GrainCancellationToken ct);
	}
}