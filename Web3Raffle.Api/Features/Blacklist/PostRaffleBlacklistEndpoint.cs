using Web3raffle.Models.Responses;
using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Blacklist;

public class PostRaffleBlacklistEndpoint : Endpoint<Web3RaffleBlacklistRequestModel, ResponseModel<Web3RaffleBlacklistModel>>
{
	private readonly IClusterClient orleansClient;

	public PostRaffleBlacklistEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Post("/raffles/{raffleId}/blacklist/{walletAddress}");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleBlacklistRequestModel req, CancellationToken ct)
	{
		var grain = this.orleansClient.GetGrain<IRaffleGrain>(Guid.NewGuid());
		var raffle = await grain.GetRaffleAsync(req.RaffleId, false, ct.ToGrainCancellationToken());

		ArgumentNullException.ThrowIfNull(raffle);

		this.ThrowIfAnyErrors();

		req.Data = new List<Web3RaffleBlacklistModel>();
		req.Data.Add(new Web3RaffleBlacklistModel()
		{
			RaffleId = req.RaffleId,
			WalletAddress = req.WalletAddress
		});

		await this.orleansClient.ProcessEvent<ICreateWeb3RaffleBlacklistEvent, Web3RaffleBlacklistModel>(req.ConnectionId, req.Data, ct.ToGrainCancellationToken());

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleBlacklistModel> { Data = req.Data! });
	}
}