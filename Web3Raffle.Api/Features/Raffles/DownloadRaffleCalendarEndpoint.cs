using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Raffles;

public class DownRaffleCalendarEndpoint : Endpoint<IdRequestModel>
{
	private readonly IClusterClient orleansClient;

	public DownRaffleCalendarEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{raffleId}/calendar/download");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(IdRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		string fileName = $"{req.RaffleId}_date.ics",
			   contentType = "calendar/text";

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		var fileBytes = await grain.DownloadCalendarAsync(req.RaffleId, ct.ToGrainCancellationToken());

		await this.SendBytesAsync(fileBytes, fileName, contentType, cancellation: ct);
	}
}