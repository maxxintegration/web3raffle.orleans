using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;
using Web3raffle.Utilities.Helpers;

namespace Web3raffle.Api.Features.Projects;

public class GetProjectsEndpoint : Endpoint<QueryModel, ResponseCollectionModel<Web3RaffleProjectModel>>
{
	private readonly IClusterClient orleansClient;
	private readonly GuidBagHelper guidBag;

	public GetProjectsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
		this.guidBag = new GuidBagHelper();
	}

	public override void Configure()
	{
		this.Get("/projects");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(QueryModel req, CancellationToken ct)
	{
		var grain = this.orleansClient.GetGrain<IProjectGrain>(this.guidBag.GetNext());

		var data = await grain.GetProjectsAsync(req, ct.ToGrainCancellationToken());

		await this.SendAsync(data.ToResponseModel(), cancellation: ct);
	}
}