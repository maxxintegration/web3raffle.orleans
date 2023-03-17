using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Activities;

public class GetActivitesEndpoint : Endpoint<QueryModel, ResponseCollectionModel<Web3RaffleEventLogModel>>
{
	private readonly IClusterClient orleansClient;

	public GetActivitesEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		//this.Get("/activities");
		this.Verbs(Http.GET);
		this.Routes("/activities");

		this.AllowAnonymous();
	}

	public override async Task HandleAsync(QueryModel req, CancellationToken ct)
	{

		var primaryKey = Guid.Parse("777BAD04-7576-4A8B-9701-43CDCBC1B98C");

		var grain = this.orleansClient.GetGrain<IEventLogGrain>(primaryKey);

		var result = await grain.GetEventLogsAsync(req, ct.ToGrainCancellationToken());

		var data = result.ToResponseModel();

		var length = await grain.GetEventLogsCountAsync(req, ct.ToGrainCancellationToken());

		data.SetLength(length);

		await this.SendAsync(data, cancellation: ct);

	}
}