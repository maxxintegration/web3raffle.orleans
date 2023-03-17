using Orleans.Concurrency;
using Web3raffle.Models.Responses;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IProcessableEventGrain : IGrainWithGuidKey
	{
		[OneWay]
		Task Notify<TRequest>(string? connectionId, TRequest request, GrainCancellationToken cancellationToken) where TRequest : BaseResponseDataModel;

		Task<bool> ActivateProcessEvent(GrainCancellationToken cancellationToken);
	}
}