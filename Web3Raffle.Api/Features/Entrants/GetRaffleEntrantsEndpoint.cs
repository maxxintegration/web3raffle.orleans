using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;
using Web3raffle.Utilities.Helpers;

namespace Web3raffle.Api.Features.Entrants;

public class GetEntrantsEndpoint : Endpoint<RaffleQueryModel, ResponseCollectionModel<Web3RaffleEntrantModel>>
{
	private readonly IClusterClient orleansClient;
	private readonly GuidBagHelper guidBag;

	public GetEntrantsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
		this.guidBag = new GuidBagHelper();
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/entrants");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IEntrantGrain>(primaryKey);

		var result = await grain.GetEntrantsAsync(req.RaffleId, req, ct.ToGrainCancellationToken());

		var data = result.ToResponseModel();

		var length = await grain.GetEntrantCountAsync(req.RaffleId, req, ct.ToGrainCancellationToken());

		data.SetLength(length);

		await this.SendAsync(data, cancellation: ct);
	}
}