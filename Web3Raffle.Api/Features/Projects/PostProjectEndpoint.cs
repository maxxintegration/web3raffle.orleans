using Web3raffle.Shared.Exceptions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Projects;

public class PostProjectEndpoint : Endpoint<Web3RaffleProjectRequestModel, ResponseModel<Web3RaffleProjectModel>>
{
	private readonly IClusterClient orleansClient;

	public PostProjectEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Post("/projects");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleProjectRequestModel req, CancellationToken ct)
	{
		var primaryKey = Guid.Parse(req.Id);

		var model = req.Clone<Web3RaffleProjectRequestModel, Web3RaffleProjectModel>();

		var grain = this.orleansClient.GetGrain<IProjectGrain>(primaryKey);

		bool isExist = await grain.IsProjectExistBySlugAsync(req.Name.ToUrlSlug(), ct.ToGrainCancellationToken());
		if (isExist)
			throw new Web3RaffleException("Project already exists.  Please choose a different one.");

		await grain.CreateProjectAsync(model, ct.ToGrainCancellationToken());

		//this.orleansClient.ProcessEvent<ICreateWeb3RaffleProjectEvent, Web3RaffleProjectModel>(req.ConnectionId, model, ct);

		await this.SendOkAsync(model.ToResponseModel(), ct);
	}
}