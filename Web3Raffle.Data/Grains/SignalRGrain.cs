using Web3raffle.Data.Hubs;
using Web3raffle.Utilities.Helpers;
using Web3raffle.Abstractions.GrainInterfaces;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = 2)]
[VFGrainPlacement]
public class SignalR : Grain, ISignalRGrain
{
	private readonly SignalRRepository<MessageHub> signalRRepository;
	private readonly ILogger<SignalR> logger;

	public SignalR(SignalRRepository<MessageHub> signalRRepository, ILogger<SignalR> logger)
	{
		this.signalRRepository = signalRRepository;
		this.logger = logger;
	}

	public async Task SendMessage<T>(string invocationType, string? connectionId, SignalREvent<T> message, GrainCancellationToken cancellationToken) where T : class
	{
		message.SetInvocationType(invocationType);
		message.SetConnectionId(connectionId);

		try
		{
			if (message.ConnectionId is not null)
			{
				this.logger.LogInformation("SendMessage w/ ConnectionID ({invocationType}/{connectionId}): {message}", invocationType, message.ConnectionId, message);

				await this
					.signalRRepository
					.SendClient(message.ConnectionId, message, cancellationToken.CancellationToken);

				return;
			}

			if (message.RecipientId is not null)
			{
				this.logger.LogInformation("SendMessage w/ RecipientID ({invocationType}/{connectionId}): {message}", invocationType, message.RecipientId, message);

				await this
					.signalRRepository
					.SendUser(message.RecipientId, message, cancellationToken.CancellationToken);

				return;
			}

			this.logger.LogInformation("SendMessage to ALL ({invocationType}): {message}", invocationType, message);

			await this
				.signalRRepository
				.SendAll(message, cancellationToken.CancellationToken);
		}
		catch (Exception ex)
		{
			this.logger.LogError("SendMessage failed! Error: {ex}", ex);
		}
	}
}