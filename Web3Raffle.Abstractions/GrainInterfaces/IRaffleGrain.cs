using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IRaffleGrain : IGrainWithGuidKey
	{
		Task<List<Web3RaffleResponseModel>> GetRafflesAsync(QueryModel queryParams, bool maskPassword, GrainCancellationToken ct);

		Task<int> GetRaffleCountAsync(QueryModel queryParams, GrainCancellationToken ct);

		Task<Web3RaffleModel> GetRaffleAsync(string raffleId, bool autoUpdateStatus, GrainCancellationToken ct);

		Task CreateRaffleAsync(Web3RaffleModel model, string? connectionId, GrainCancellationToken ct);

		Task UpdateRaffleAsync(Web3RaffleModel mode, GrainCancellationToken ct);

		Task DeleteRaffleAsync(string id, GrainCancellationToken ct);

		Task BeginRaffleAsync(string id, GrainCancellationToken ct);

		Task EndRaffleAsync(string id, GrainCancellationToken ct);

		Task DrawRaffleAsync(string id, GrainCancellationToken ct);

		Task ValidateRaffleAsync(Web3RaffleModel raffle, bool newRaffle, GrainCancellationToken ct);

		Task<Web3RaffleModel> ValidateRaffleBeforeEnterAsync(string raffleId, List<Web3RaffleEntrantModel> entrants, string? rafflePassword, GrainCancellationToken ct);

		Task<byte[]> DownloadCalendarAsync(string id, GrainCancellationToken ct);
	}
}