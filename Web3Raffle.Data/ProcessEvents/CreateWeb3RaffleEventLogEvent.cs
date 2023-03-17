using Web3raffle.Abstractions;
using Web3raffle.Models.Data;

namespace Web3raffle.Data.ProcessEvents;

[CollectionAgeLimit(Minutes = 10)]
[VFGrainPlacement]
public class CreateWeb3RaffleEventLogEvent : GrainFactoryWithNotifications, ICreateWeb3RaffleEventLogEvent
{
	private readonly IGrainFactory grainFactory;
	private readonly ILogger<CreateWeb3RaffleEventLogEvent> logger;
	private readonly IConfiguration configuration;

	public CreateWeb3RaffleEventLogEvent(IGrainFactory grainFactory, ILogger<CreateWeb3RaffleEventLogEvent> logger, IConfiguration configuration) : base(grainFactory)
	{
		this.grainFactory = grainFactory;
		this.logger = logger;
		this.configuration = configuration;
	}

	public Task<bool> ActivateProcessEvent(GrainCancellationToken cancellationToken)
	{
		return Task.FromResult(true);
	}

	public async Task Notify<TRequest>(string? connectionId, TRequest request, GrainCancellationToken cancellationToken) where TRequest : BaseResponseDataModel
	{
		if (request is not Web3RaffleEventLogModel requestModel)
		{
			return;
		}

		var primaryKey = Guid.Parse(string.IsNullOrEmpty(requestModel.RaffleId) ? requestModel.Id : requestModel.RaffleId);


		//this.OnBeforeExecution(connectionId, new SignalREvent<TRequest>(
		//	ProcessName: nameof(CreateWeb3RaffleEventLogEvent),
		//	RecipientId: requestModel.CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: request
		//), cancellationToken);

		try
		{
			var grain = this.grainFactory.GetGrain<IEventLogGrain>(primaryKey);

			requestModel.Id = Guid.NewGuid().ToString();
			requestModel.CreatedAt = DateTimeOffset.UtcNow;
			requestModel.ModifiedAt = requestModel.CreatedAt;

			await grain.CreateEventLogAsync(requestModel, cancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("An error occured: {ex}", ex);

			//this.OnAfterExecution(connectionId, new SignalREvent<TRequest>(
			//	ProcessName: nameof(CreateWeb3RaffleEventLogEvent),
			//	RecipientId: requestModel.CreatedBy ?? "",
			//	GrainKey: primaryKey,
			//	Payload: request,
			//	Error: ex.Message
			//), cancellationToken);

			return;
		}

		//this.OnAfterExecution(connectionId, new SignalREvent<Web3RaffleEventLogModel>(
		//	ProcessName: nameof(CreateWeb3RaffleEventLogEvent),
		//	RecipientId: requestModel.CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: requestModel
		//), cancellationToken);
	}
}