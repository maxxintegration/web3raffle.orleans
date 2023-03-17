using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Whitelist;

public class GetWhitelistsEndpoint : Endpoint<RaffleQueryModel, ResponseCollectionModel<Web3RaffleWhitelistModel>>
{
	private readonly IClusterClient orleansClient;

	public GetWhitelistsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/whitelist");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IWhitelistGrain>(primaryKey);

		var data = await grain.GetWhitelistAsync(req.RaffleId, ct.ToGrainCancellationToken());

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleWhitelistModel> { Data = data });
	}
}