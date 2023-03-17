using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Activities;

public class GetActivityByRaffleEndPoint : Endpoint<RaffleQueryModel, ResponseCollectionModel<Web3RaffleEventLogModel>>
{
	private readonly IClusterClient orleansClient;

	public GetActivityByRaffleEndPoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		//this.Get("/activities");
		this.Verbs(Http.GET);
		this.Routes("/activities/{raffleId}");

		this.AllowAnonymous();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IEventLogGrain>(primaryKey);

		var result = await grain.GetEventLogsAsync(req.RaffleId, req, ct.ToGrainCancellationToken());

		var data = result.ToResponseModel();

		var length = await grain.GetEventLogsCountAsync(req.RaffleId, req, ct.ToGrainCancellationToken());

		data.SetLength(length);

		await this.SendAsync(data, cancellation: ct);

	}
}