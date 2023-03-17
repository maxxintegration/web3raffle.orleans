using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Utilities.Repositories.SignalR
{
	public class SignalRRepository<T> where T : Hub
	{
		private const string ReceiveMessage = nameof(ReceiveMessage);

		private readonly IHubContext<T> hub;

		public SignalRRepository(IHubContext<T> hub)
		{
			this.hub = hub;
		}

		public async Task SendAll<TMessage>(TMessage messageObject, CancellationToken cancellationToken) where TMessage : class
		{
			await this.hub.Clients
				.All
				.SendAsync(ReceiveMessage, messageObject, cancellationToken: cancellationToken);
		}

		public async Task SendClient<TMessage>(string connectionId, TMessage messageObject, CancellationToken cancellationToken) where TMessage : class
		{
			await this.hub.Clients
				.Client(connectionId)
				.SendAsync(ReceiveMessage, messageObject, cancellationToken: cancellationToken);
		}

		public async Task SendUser<TMessage>(string userId, TMessage messageObject, CancellationToken cancellationToken) where TMessage : class
		{
			await this.hub.Clients
				.User(userId)
				.SendAsync(ReceiveMessage, messageObject, cancellationToken: cancellationToken);
		}
	}
}