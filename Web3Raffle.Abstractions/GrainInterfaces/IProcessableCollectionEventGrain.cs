using Orleans.Concurrency;
using Web3raffle.Models.Responses;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IProcessableCollectionEventGrain : IGrainWithGuidKey
	{
		[OneWay]
		Task Notify<TRequest>(string? connectionId, List<TRequest> request, GrainCancellationToken cancellationToken) where TRequest : BaseResponseDataModel;

		Task<bool> ActivateProcessEvent(GrainCancellationToken cancellationToken);
	}
}