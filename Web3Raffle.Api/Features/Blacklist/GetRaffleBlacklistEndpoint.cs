using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Blacklist;

public class GetBlacklistEndpoint : Endpoint<RaffleQueryModel, ResponseCollectionModel<Web3RaffleBlacklistModel>>
{
	private readonly IClusterClient orleansClient;

	public GetBlacklistEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/blacklist");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		var grain = this.orleansClient.GetGrain<IBlacklistGrain>(Guid.NewGuid());

		var data = await grain.GetBlacklistAsync(req.RaffleId!, ct.ToGrainCancellationToken());

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleBlacklistModel> { Data = data });
	}
}