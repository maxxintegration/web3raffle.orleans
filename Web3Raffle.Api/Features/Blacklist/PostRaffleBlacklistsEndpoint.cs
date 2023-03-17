using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Blacklist;

public class PostRaffleBlacklistsEndpoint : Endpoint<Web3RaffleBlacklistRequestModel, ResponseCollectionModel<Web3RaffleBlacklistModel>>
{
	private readonly IClusterClient orleansClient;

	public PostRaffleBlacklistsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Post("/raffles/{raffleId}/blacklist");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleBlacklistRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.Data);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);
		var raffle = await grain.GetRaffleAsync(req.RaffleId, false, ct.ToGrainCancellationToken());

		ArgumentNullException.ThrowIfNull(raffle);

		this.ThrowIfAnyErrors();

		foreach (var item in req.Data)
		{
			item.RaffleId = req.RaffleId;
		}

		await this.orleansClient.ProcessEvent<ICreateWeb3RaffleBlacklistEvent, Web3RaffleBlacklistModel>(req.ConnectionId, req.Data, ct.ToGrainCancellationToken());

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleBlacklistModel> { Data = req.Data! });
	}
}