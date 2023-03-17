using Web3raffle.Abstractions;
using Web3raffle.Models.Data;

namespace Web3raffle.Data.ProcessEvents;

[CollectionAgeLimit(Minutes = 10)]
[VFGrainPlacement]
public class DeleteWeb3RaffleWhitelistEvent : GrainFactoryWithNotifications, IDeleteWeb3RaffleWhitelistEvent
{
	private readonly IGrainFactory grainFactory;
	private readonly ILogger<DeleteWeb3RaffleWhitelistEvent> logger;
	private readonly IConfiguration configuration;

	public DeleteWeb3RaffleWhitelistEvent(IGrainFactory grainFactory, ILogger<DeleteWeb3RaffleWhitelistEvent> logger, IConfiguration configuration) : base(grainFactory)
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

		var primaryKey = Guid.Parse(requestModels[0].RaffleId);

		//this.OnBeforeExecution(connectionId, new SignalREvent<TRequest>(
		//	ProcessName: nameof(DeleteWeb3RaffleWhitelistEvent),
		//	RecipientId: requestModels[0].CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: request[0]
		//), cancellationToken);

		try
		{
			var grain = this.grainFactory.GetGrain<IWhitelistGrain>(primaryKey);
			await grain.DeleteWhitelistAsync(requestModels, cancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("An error occured: {ex}", ex);

			//this.OnAfterExecution(connectionId, new SignalREvent<TRequest>(
			//	ProcessName: nameof(DeleteWeb3RaffleWhitelistEvent),
			//	RecipientId: requestModels[0].CreatedBy ?? "",
			//	GrainKey: primaryKey,
			//	Payload: request[0],
			//	Error: ex.Message
			//), cancellationToken);

			return;
		}

		//this.OnAfterExecution(connectionId, new SignalREvent<Web3RaffleWhitelistModel>(
		//	ProcessName: nameof(DeleteWeb3RaffleWhitelistEvent),
		//	RecipientId: requestModels[0].CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: requestModels[0]
		//), cancellationToken);
	}
}