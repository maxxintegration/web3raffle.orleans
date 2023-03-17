using Web3raffle.Abstractions;
using Web3raffle.Models.Data;

namespace Web3raffle.Data.ProcessEvents;

[CollectionAgeLimit(Minutes = 10)]
[VFGrainPlacement]
public class CreateWeb3RaffleWhitelistEvent : GrainFactoryWithNotifications, ICreateWeb3RaffleWhitelistEvent
{
	private readonly IGrainFactory grainFactory;
	private readonly ILogger<CreateWeb3RaffleWhitelistEvent> logger;
	private readonly IConfiguration configuration;

	public CreateWeb3RaffleWhitelistEvent(IGrainFactory grainFactory, ILogger<CreateWeb3RaffleWhitelistEvent> logger, IConfiguration configuration) : base(grainFactory)
	{
		this.grainFactory = grainFactory;
		this.logger = logger;
		this.configuration = configuration;
	}

	public Task<bool> ActivateProcessEvent(GrainCancellationToken cancellationToken)
	{
		return Task.FromResult(true);
	}

	public async Task Notify<TRequest>(string? connectionId, List<TRequest> request, GrainCancellationToken cancellationToken) where TRequest : BaseResponseDataModel
	{
		if (request is not List<Web3RaffleWhitelistModel> requestModels)
		{
			return;
		}

		var grainKey = Guid.Parse(requestModels[0].RaffleId);

		//this.OnBeforeExecution(connectionId, new SignalREvent<TRequest>(
		//	ProcessName: nameof(CreateWeb3RaffleWhitelistEvent),
		//	RecipientId: requestModels[0].CreatedBy ?? "",
		//	GrainKey: grainKey,
		//	Payload: request[0]
		//), cancellationToken);

		try
		{
			var grain = this.grainFactory.GetGrain<IWhitelistGrain>(grainKey);
			await grain.CreateWhitelistAsync(requestModels, cancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("An error occured: {ex}", ex);

			//this.OnAfterExecution(connectionId, new SignalREvent<TRequest>(
			//	ProcessName: nameof(CreateWeb3RaffleWhitelistEvent),
			//	RecipientId: requestModels[0].CreatedBy ?? "",
			//	GrainKey: grainKey,
			//	Payload: request[0],
			//	Error: ex.Message
			//), cancellationToken);

			return;
		}

		//this.OnAfterExecution(connectionId, new SignalREvent<Web3RaffleWhitelistModel>(
		//	ProcessName: nameof(CreateWeb3RaffleWhitelistEvent),
		//	RecipientId: requestModels[0].CreatedBy ?? "",
		//	GrainKey: grainKey,
		//	Payload: requestModels[0]
		//), cancellationToken);
	}
}