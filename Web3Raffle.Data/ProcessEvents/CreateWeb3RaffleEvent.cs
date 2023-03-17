using Web3raffle.Abstractions;
using Web3raffle.Models.Data;

namespace Web3raffle.Data.ProcessEvents;

[CollectionAgeLimit(Minutes = 10)]
[VFGrainPlacement]
public class CreateWeb3RaffleEvent : GrainFactoryWithNotifications, ICreateWeb3RaffleEvent
{
	private readonly IGrainFactory grainFactory;
	private readonly ILogger<CreateWeb3RaffleEvent> logger;
	private readonly IConfiguration configuration;

	public CreateWeb3RaffleEvent(IGrainFactory grainFactory, ILogger<CreateWeb3RaffleEvent> logger, IConfiguration configuration) : base(grainFactory)
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
		if (request is not Web3RaffleModel requestModel)
		{
			return;
		}

		var primaryKey = Guid.Parse(requestModel.Id);

		//this.OnBeforeExecution(connectionId, new SignalREvent<TRequest>(
		//	ProcessName: nameof(CreateWeb3RaffleEvent),
		//	RecipientId: requestModel.CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: request
		//), cancellationToken);

		try
		{
			var grain = this.grainFactory.GetGrain<IRaffleGrain>(primaryKey);

			if (!requestModel.CustomRandomizerSeed || requestModel.RandomizeSeed < 1)
				requestModel.RandomizeSeed = new CryptoRandom().Next();

			await grain.CreateRaffleAsync(requestModel, connectionId, cancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("An error occured: {ex}", ex);

			//this.OnAfterExecution(connectionId, new SignalREvent<TRequest>(
			//	ProcessName: nameof(CreateWeb3RaffleEvent),
			//	RecipientId: requestModel.CreatedBy ?? "",
			//	GrainKey: primaryKey,
			//	Payload: request,
			//	Error: ex.Message
			//), cancellationToken);

			return;
		}

		//this.OnAfterExecution(connectionId, new SignalREvent<Web3RaffleModel>(
		//	ProcessName: nameof(CreateWeb3RaffleEvent),
		//	RecipientId: requestModel.CreatedBy ?? "",
		//	GrainKey: primaryKey,
		//	Payload: requestModel
		//), cancellationToken);
	}
}