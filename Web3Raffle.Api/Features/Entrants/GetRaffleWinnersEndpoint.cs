using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Entrants;

public class GetRaffleWinnersEndpoint : Endpoint<RaffleQueryModel, ResponseCollectionModel<Web3RaffleEntrantModel>>
{
	private readonly IClusterClient orleansClient;

	public GetRaffleWinnersEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/winners");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IEntrantGrain>(primaryKey);

		var result = await grain.GetWinningEntrantsAsync(req.RaffleId, req, ct.ToGrainCancellationToken());

		var data = result.ToResponseModel();

		var length = await grain.GetWinningEntrantsCountAsync(req.RaffleId, ct.ToGrainCancellationToken());

		data.SetLength(length);

		await this.SendAsync(data, cancellation: ct);
	}
}