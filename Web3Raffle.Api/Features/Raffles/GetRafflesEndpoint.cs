using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;
using Web3raffle.Utilities.Helpers;

namespace Web3raffle.Api.Features.Raffles;

public class GetRafflesEndpoint : Endpoint<QueryModel, ResponseCollectionModel<Web3RaffleResponseModel>>
{
	private readonly IClusterClient orleansClient;
	private readonly GuidBagHelper guidBag;

	public GetRafflesEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
		this.guidBag = new GuidBagHelper();
	}

	public override void Configure()
	{
		this.Get("/raffles");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(QueryModel req, CancellationToken ct)
	{
		var primaryKey = this.guidBag.GetNext();

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		var result = await grain.GetRafflesAsync(req, true, ct.ToGrainCancellationToken());

		var data = result.ToResponseModel();

		var length = await grain.GetRaffleCountAsync(req, ct.ToGrainCancellationToken());

		data.SetLength(length);

		await this.SendAsync(data, cancellation: ct);
	}
}