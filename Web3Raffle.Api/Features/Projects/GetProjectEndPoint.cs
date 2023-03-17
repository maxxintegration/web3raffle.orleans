using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Projects;

public class GetProjectEndpoint : Endpoint<IdRequestModel, ResponseModel<Web3RaffleProjectModel>>
{
	private readonly IClusterClient orleansClient;

	public GetProjectEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/projects/{id}");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(IdRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req.Id);

		var container = this.orleansClient.GetGrain<IProjectGrain>(Guid.Parse(req.Id));

		var data = await container.GetProjectAsync(req.Id, ct.ToGrainCancellationToken());

		await this.SendAsync(data.ToResponseModel(), cancellation: ct);
	}
}