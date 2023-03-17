using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Whitelist;

public class DeleteRaffleWhitelistEndpoint : Endpoint<Web3RaffleWhitelistRequestModel, EmptyResponse>
{
	private readonly IClusterClient orleansClient;

	public DeleteRaffleWhitelistEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Delete("/raffles/{raffleId}/whitelist");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleWhitelistRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IWhitelistGrain>(primaryKey);
		req.Data = await grain.GetWhitelistAsync(req.RaffleId, ct.ToGrainCancellationToken());

		await this.orleansClient.ProcessEvent<IDeleteWeb3RaffleWhitelistEvent, Web3RaffleWhitelistModel>(req.ConnectionId, req.Data, ct.ToGrainCancellationToken());

		await this.SendAsync(new EmptyResponse(), 200, ct);
	}
}