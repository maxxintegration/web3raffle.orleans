using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Responses;

namespace Web3raffle.Abstractions
{
	public static class ProcessEventExtensions
	{
		public static async Task ProcessEvent<IProcess, TModel>(this IClusterClient clusterClient, string? connectionId, string? userId, TModel requestModel, GrainCancellationToken ct)
			where IProcess : class, IProcessableEventGrain
			where TModel : BaseResponseDataModel
		{
			var grainkey = Guid.Parse(requestModel.Id);
			if (userId is not null)
			{
				grainkey = Guid.Parse(userId);
			}

			var grain = clusterClient
				.GetGrain<IProcess>(grainkey);

			await grain.ActivateProcessEvent(ct);
			await grain.Notify(connectionId, requestModel, ct);
		}

		public static async Task ProcessEvent<IProcess, TModel>(this IClusterClient clusterClient, string? connectionId, TModel requestModel, GrainCancellationToken ct)
			where IProcess : class, IProcessableEventGrain
			where TModel : BaseResponseDataModel
		{
			var grain = clusterClient
				.GetGrain<IProcess>(Guid.Parse(requestModel.Id));

			await grain.ActivateProcessEvent(ct);
			await grain.Notify(connectionId, requestModel, ct);
		}

		public static async Task ProcessEvent<IProcess, TModel>(this IGrainFactory grainFactory, string? connectionId, TModel requestModel, GrainCancellationToken ct)
			where IProcess : class, IProcessableEventGrain
			where TModel : BaseResponseDataModel
		{
			var grain = grainFactory
				.GetGrain<IProcess>(Guid.Parse(requestModel.Id));

			await grain.ActivateProcessEvent(ct);
			await grain.Notify(connectionId, requestModel, ct);
		}

		public static async Task ProcessEvent<IProcess, TModel>(this IGrainFactory grainFactory, string? connectionId, List<TModel> requestModels, GrainCancellationToken ct)
			where IProcess : class, IProcessableCollectionEventGrain
			where TModel : BaseResponseDataModel
		{
			var grain = grainFactory
				.GetGrain<IProcess>(Guid.NewGuid());

			await grain.ActivateProcessEvent(ct);
			await grain.Notify(connectionId, requestModels, ct);
		}
	}
}