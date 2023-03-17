using Web3raffle.Abstractions;
using Web3raffle.Models.Enums;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Data;
using Web3raffle.Shared;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class EventLogGrain : Grain, IEventLogGrain
{

	public void LogThis(string raffleId, EventLogType logType, string message, GrainCancellationToken ct)
	{
		var eventLog = new Web3RaffleEventLogModel()
		{
			RaffleId = raffleId,
			LogType = logType,
			LogMessage = message
		};

		this.GrainFactory.ProcessEvent<ICreateWeb3RaffleEventLogEvent, Web3RaffleEventLogModel>(null, eventLog, ct);
	}


	public async Task CreateEventLogAsync(Web3RaffleEventLogModel model, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(Guid.NewGuid());
		await container.Write(model, ct);
	}

	public async Task<List<Web3RaffleEventLogModel>> GetEventLogsAsync(QueryModel queryParams, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(Guid.NewGuid());

		var data = await container.Read(queryParams, ct);

		return data;
	}

	public async Task<int> GetEventLogsCountAsync(QueryModel queryParams, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(Guid.NewGuid());

		return await container.Count(queryParams, ct);
	}

	public async Task<List<Web3RaffleEventLogModel>> GetEventLogsAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(Guid.NewGuid());

		queryParams.AppendFilter($"raffleId eq '{raffleId}'");

		var data = await container.Read(queryParams, ct);

		return data;
	}

	public async Task<int> GetEventLogsCountAsync(string raffleId, QueryModel queryParams, GrainCancellationToken ct)
	{
		var container = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(Guid.NewGuid());

		queryParams.AppendFilter($"raffleId eq '{raffleId}'");

		return await container.Count(queryParams, ct);
	}

	public async Task DeleteEventLogsAsync(string raffleId, GrainCancellationToken ct)
	{
		var queryParams = new QueryModel() { Top = 1000000 };
		var grain = this.GrainFactory.GetGrain<ICosmosDbGrain<Web3RaffleEventLogModel>>(this.GetPrimaryKey());

		var eventLogs = await this.GetEventLogsAsync(raffleId, queryParams, ct);

		foreach (var eventLog in eventLogs)
		{
			await grain.Delete(eventLog.Id, ct);
		}
	}
}