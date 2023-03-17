using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Utilities.Helpers;

namespace Web3raffle.Abstractions
{
	public abstract class GrainFactoryWithNotifications : Grain
	{
		private readonly IGrainFactory grainFactory;

		public GrainFactoryWithNotifications(IGrainFactory grainFactory)
		{
			this.grainFactory = grainFactory;
		}

		public virtual void OnBeforeExecution<T>(string? connectionId, SignalREvent<T> signalREvent, GrainCancellationToken cancellationToken) where T : class
		{
			var signalR = this.grainFactory.GetGrain<ISignalRGrain>(signalREvent.GrainKey);

			signalR.SendMessage(nameof(OnBeforeExecution), connectionId, signalREvent, cancellationToken);
		}

		public virtual void OnAfterExecution<T>(string? connectionId, SignalREvent<T> signalREvent, GrainCancellationToken cancellationToken) where T : class
		{
			var signalR = this.grainFactory.GetGrain<ISignalRGrain>(signalREvent.GrainKey);

			signalR.SendMessage(nameof(OnAfterExecution), connectionId, signalREvent, cancellationToken);
		}
	}
}