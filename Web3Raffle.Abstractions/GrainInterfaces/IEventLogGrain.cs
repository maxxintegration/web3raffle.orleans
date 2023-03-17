using Web3raffle.Models.Data;
using Web3raffle.Models.Enums;
using Web3raffle.Models.Requests;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IEventLogGrain : IGrainWithGuidKey
	{
		void LogThis(string raffleId, EventLogType logType, string message, GrainCancellationToken ct);

		Task CreateEventLogAsync(Web3RaffleEventLogModel model, GrainCancellationToken cancellationToken);

		Task<List<Web3RaffleEventLogModel>> GetEventLogsAsync(QueryModel queryParams, GrainCancellationToken ct);

		Task<int> GetEventLogsCountAsync(QueryModel queryParams, GrainCancellationToken ct);

		Task<List<Web3RaffleEventLogModel>> GetEventLogsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct);

		Task<int> GetEventLogsCountAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct);

		Task DeleteEventLogsAsync(string raffleId, GrainCancellationToken ct);
	}
}