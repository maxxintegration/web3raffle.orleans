using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Entrants;

public class DownloadRaffleEntrantsEndpoint : Endpoint<IdRequestModel>
{
	private readonly IClusterClient orleansClient;

	public DownloadRaffleEntrantsEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/entrants/download");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(IdRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		string fileName = $"{req.RaffleId}_source.csv",
			   contentType = "text/csv";

		var grain = this.orleansClient.GetGrain<IEntrantGrain>(primaryKey);

		var fileBytes = await grain.DownloadEntrantsAsync(req.RaffleId, ct.ToGrainCancellationToken());

		await this.SendBytesAsync(fileBytes, fileName, contentType);
	}
}