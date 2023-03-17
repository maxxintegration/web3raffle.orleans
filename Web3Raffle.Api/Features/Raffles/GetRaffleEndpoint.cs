using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Raffles;

public class GetRaffleEndpoint : Endpoint<IdRequestModel, ResponseModel<Web3RaffleModel>>
{
	private readonly IClusterClient orleansClient;

	public GetRaffleEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Get("/raffles/{id}");
		this.AllowAnonymous();

		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(IdRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.Id);

		var primaryKey = Guid.Parse(req.Id);

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		var data = await grain.GetRaffleAsync(req.Id, true, ct.ToGrainCancellationToken());

		// MASK PASSWORD
		if (data != null)
			data.Password = data.Password.MaskString(data.PasswordProtect);

		await this.SendAsync(data!.ToResponseModel(), cancellation: ct);
	}
}