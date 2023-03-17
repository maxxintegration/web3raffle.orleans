using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Blacklist;

public class DeleteRaffleBlackListAddressEndpointcs : Endpoint<Web3RaffleBlacklistRequestModel, EmptyResponse>
{
	private readonly IClusterClient orleansClient;

	public DeleteRaffleBlackListAddressEndpointcs(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Delete("/raffles/{raffleId}/blacklist/{walletAddress}");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleBlacklistRequestModel req, CancellationToken ct)
	{
		var grain = this.orleansClient.GetGrain<IBlacklistGrain>(Guid.Parse(req.RaffleId));
		var blacklist = await grain.GetBlacklistAsync(req.RaffleId, req.WalletAddress, ct.ToGrainCancellationToken());

		req.Data = new List<Web3RaffleBlacklistModel>();
		if (blacklist != null)
			req.Data.Add(blacklist);

		await this.orleansClient.ProcessEvent<IDeleteWeb3RaffleBlacklistEvent, Web3RaffleBlacklistModel>(req.ConnectionId, req.Data, ct.ToGrainCancellationToken());

		await this.SendAsync(new EmptyResponse(), 200, ct);
	}
}