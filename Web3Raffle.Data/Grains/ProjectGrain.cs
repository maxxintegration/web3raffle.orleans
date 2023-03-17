using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Shared;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = SettingParams.COLLECTION_AGE_LIMIT_MINUTES)]
[VFGrainPlacement]
public class ProjectGrain : Grain, IProjectGrain
{
	public async Task<List<Web3RaffleProjectModel>> GetProjectsAsync(QueryModel queryParams, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory
			.GetGrain<ICosmosDbGrain<Web3RaffleProjectModel>>(this.GetPrimaryKey());

		queryParams.AppendFilter("deleted = false");

		return await grain.Read(queryParams, ct);
	}

	public async Task<Web3RaffleProjectModel> GetProjectAsync(string projectId, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory
			.GetGrain<ICosmosDbGrain<Web3RaffleProjectModel>>(this.GetPrimaryKey());

		return await grain
			.Read(projectId.ToLower(), ct);
	}

	public async Task<bool> IsProjectExistBySlugAsync(string urlSlug, GrainCancellationToken ct)
	{
		var project = await this.GetProjectBySlugAsync(urlSlug, ct);
		return project != null;
	}

	public async Task<bool> IsProjectExistByIdAsync(string projectId, GrainCancellationToken ct)
	{
		var project = await this.GetProjectAsync(projectId, ct);
		return project != null;
	}

	public async Task<Web3RaffleProjectModel> GetProjectBySlugAsync(string urlSlug, GrainCancellationToken ct)
	{
		Web3RaffleProjectModel project = default!;
		var queryParams = new QueryModel()
		{
			Filter = $"urlSlug = '{urlSlug}'"
		};

		var projects = await this.GetProjectsAsync(queryParams, ct);

		if (projects != null && projects.Count > 0)
			project = projects[0];

		return project;
	}

	public async Task CreateProjectAsync(Web3RaffleProjectModel model, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory
			.GetGrain<ICosmosDbGrain<Web3RaffleProjectModel>>(this.GetPrimaryKey());

		model.UrlSlug = model.Name.ToUrlSlug();

		await grain.Write(model, ct);
	}

	public async Task UpdateProjectAsync(Web3RaffleProjectModel model, GrainCancellationToken ct)
	{
		var grain = this.GrainFactory
			.GetGrain<ICosmosDbGrain<Web3RaffleProjectModel>>(this.GetPrimaryKey());

		var project = await this.GetProjectAsync(model.Id, ct);

		model.UrlSlug = model.Name.ToUrlSlug();
		model.CreatedBy = project.CreatedBy;
		model.Tags = project.Tags;
		model.CreatedAt = project.CreatedAt;
		model.ModifiedAt = DateTimeOffset.UtcNow;
		model.Deleted = false;

		await grain.Update(model, ct);
	}
}