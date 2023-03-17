using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Api.ExternalServices.IServices;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Entrants;

public class PostRaffleEntrantEndpoint : Endpoint<Web3RaffleEntrantRequestModel, ResponseCollectionModel<Web3RaffleEntrantModel>>
{
	private readonly IClusterClient orleansClient;
	private readonly IExternalProfileService externalProfileService;

	public PostRaffleEntrantEndpoint(IClusterClient orleansClient, IExternalProfileService externalProfileService)
	{
		this.orleansClient = orleansClient;
		this.externalProfileService = externalProfileService;
	}

	public override void Configure()
	{
		this.Post("/raffles/{raffleId}/entrants");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleEntrantRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);
		ArgumentNullException.ThrowIfNull(req.Data);

		var primaryKey = Guid.Parse(req.RaffleId.ToLower());
		//var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		// VALIDATE RAFFLE BEFORE ENTER
		// await grain.ValidateRaffleBeforeEnterAsync(req.RaffleId, req.Data, req.Password, ct);

		foreach (var item in req.Data)
		{
			item.RaffleId = req.RaffleId;
			item.RafflePassword = req.Password;
		}

		// FIRE AND FORGET THIS WHEN MORE THAN 1 ENTRANT
		//if (req.Data.Count > 1)
		//	this.orleansClient.ProcessEvent<ICreateWeb3RaffleEntrantEvent, Web3RaffleEntrantModel>(req.ConnectionId, req.Data, ct);
		//else
		//{
		var entrantGrain = this.orleansClient.GetGrain<IEntrantGrain>(primaryKey);
		await entrantGrain.CreateEntrantAsync(req.Data, ct.ToGrainCancellationToken());
		//}

		await this.SendAsync(new ResponseCollectionModel<Web3RaffleEntrantModel> { Data = req.Data! });
	}
}