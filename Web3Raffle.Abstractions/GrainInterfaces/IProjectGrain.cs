using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;

namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IProjectGrain : IGrainWithGuidKey
	{
		Task<List<Web3RaffleProjectModel>> GetProjectsAsync(QueryModel queryParams, GrainCancellationToken ct);

		Task<Web3RaffleProjectModel> GetProjectAsync(string projectId, GrainCancellationToken ct);

		Task<Web3RaffleProjectModel> GetProjectBySlugAsync(string urlSlug, GrainCancellationToken ct);

		Task<bool> IsProjectExistBySlugAsync(string urlSlug, GrainCancellationToken ct);

		Task<bool> IsProjectExistByIdAsync(string projectId, GrainCancellationToken ct);

		Task CreateProjectAsync(Web3RaffleProjectModel model, GrainCancellationToken ct);

		Task UpdateProjectAsync(Web3RaffleProjectModel model, GrainCancellationToken ct);
	}
}