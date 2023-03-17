using Web3raffle.Shared.Exceptions;
using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Whitelist;

public class PostRaffleWhitelistsEndpoint : Endpoint<Web3RaffleWhitelistRequestModel, ResponseCollectionModel<Web3RaffleWhitelistModel>>
{
	private readonly IClusterClient orleansClient;

	public PostRaffleWhitelistsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Post("/raffles/{raffleId}/whitelist");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleWhitelistRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);
		ArgumentNullException.ThrowIfNull(req.Data);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);
		var raffle = await grain.GetRaffleAsync(req.RaffleId, false, ct.ToGrainCancellationToken());

		if (raffle == null)
			throw new Web3RaffleException("Invalid RaffleId!");

		foreach (var item in req.Data)
		{
			item.RaffleId = req.RaffleId;
		}

		await this.orleansClient.ProcessEvent<ICreateWeb3RaffleWhitelistEvent, Web3RaffleWhitelistModel>(req.ConnectionId, req.Data, ct.ToGrainCancellationToken());

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleWhitelistModel> { Data = req.Data });
	}
}