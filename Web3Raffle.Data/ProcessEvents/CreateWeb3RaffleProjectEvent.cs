using Web3raffle.Abstractions;
using Web3raffle.Models.Data;

namespace Web3raffle.Data.ProcessEvents;

[CollectionAgeLimit(Minutes = 10)]
[VFGrainPlacement]
public class CreateWeb3RaffleProjectEvent : GrainWithNotifications, ICreateWeb3RaffleProjectEvent
{
	private readonly IClusterClient orleansClient;
	private readonly ILogger<CreateWeb3RaffleProjectEvent> logger;
	private readonly IConfiguration configuration;

	public CreateWeb3RaffleProjectEvent(IClusterClient orleansClient, ILogger<CreateWeb3RaffleProjectEvent> logger, IConfiguration configuration) : base(orleansClient)
	{
		this.orleansClient = orleansClient;
		this.logger = logger;
		this.configuration = configuration;
	}

	public Task<bool> ActivateProcessEvent(GrainCancellationToken cancellationToken)
	{
		return Task.FromResult(true);
	}

	public async Task Notify<TRequest>(string? connectionId, TRequest request, GrainCancellationToken cancellationToken) where TRequest : BaseResponseDataModel
	{
		if (request is not Web3RaffleProjectModel requestModel)
		{
			return;
		}

		var primaryKey = Guid.Parse(requestModel.Id);

		this.OnBeforeExecution(connectionId, new SignalREvent<TRequest>(
			ProcessName: nameof(CreateWeb3RaffleProjectEvent),
			RecipientId: requestModel.CreatedBy ?? "",
			GrainKey: primaryKey,
			Payload: request
		), cancellationToken);

		try
		{
			var grain = this.orleansClient.GetGrain<IProjectGrain>(primaryKey);
			await grain.CreateProjectAsync(requestModel, cancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("An error occured: {ex}", ex);

			this.OnAfterExecution(connectionId, new SignalREvent<TRequest>(
				ProcessName: nameof(CreateWeb3RaffleProjectEvent),
				RecipientId: requestModel.CreatedBy ?? "",
				GrainKey: primaryKey,
				Payload: request,
				Error: ex.Message
			), cancellationToken);

			return;
		}

		this.OnAfterExecution(connectionId, new SignalREvent<Web3RaffleProjectModel>(
			ProcessName: nameof(CreateWeb3RaffleProjectEvent),
			RecipientId: requestModel.CreatedBy ?? "",
			GrainKey: primaryKey,
			Payload: requestModel
		), cancellationToken);
	}
}