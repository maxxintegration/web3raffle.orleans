using Orleans.Concurrency;
using Web3raffle.Utilities.Helpers;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface ISignalRGrain : IGrainWithGuidKey
	{
		[OneWay]
		Task SendMessage<T>(string invocationType, string? connectionId, SignalREvent<T> message, GrainCancellationToken cancellationToken) where T : class;
	}
}