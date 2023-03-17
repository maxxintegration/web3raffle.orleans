using Web3raffle.Models.Requests;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface ICosmosDbGrain<T> : IGrainWithGuidKey
	{
		public Task<int> Count(QueryModel queryParams, GrainCancellationToken ct);

		public Task<T> Read(string id, GrainCancellationToken ct);

		public Task<List<T>> Read(QueryModel queryParams, GrainCancellationToken ct);

		public Task<T> Write(T modelToCreate, GrainCancellationToken ct);

		public Task<T> Update(T modelToUpdate, GrainCancellationToken ct);

		public Task<T> Delete(string id, GrainCancellationToken ct);

		public Task<List<T>> Query(string sqlQuery, GrainCancellationToken ct);
	}
}