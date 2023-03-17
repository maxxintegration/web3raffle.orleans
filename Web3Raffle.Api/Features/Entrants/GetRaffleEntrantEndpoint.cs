using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Entrants;

public class GetRaffleEntrantEndpoint : Endpoint<RaffleQueryModel, ResponseModel<Web3RaffleEntrantModel>>
{
	private readonly IClusterClient orleansClient;

	public GetRaffleEntrantEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/entrants/{entrantId}");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);
		ArgumentNullException.ThrowIfNull(req.EntrantId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IEntrantGrain>(primaryKey);

		var data = await grain.GetEntrantAsync(req.EntrantId, ct.ToGrainCancellationToken());

		await this.SendAsync(data.ToResponseModel(), cancellation: ct);
	}
}