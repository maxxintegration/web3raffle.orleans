using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Raffles;

public class PutRaffleEndpoint : Endpoint<Web3RaffleRequestModel, ResponseModel<Web3RaffleModel>>
{
	private readonly IClusterClient orleansClient;

	public PutRaffleEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Put("/raffles");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(Web3RaffleRequestModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req.Id);
		ArgumentNullException.ThrowIfNull(req.ProjectId);

		var model = req.Clone<Web3RaffleRequestModel, Web3RaffleModel>();
		var primaryKey = Guid.Parse(model.Id);

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		await grain.ValidateRaffleAsync(model, false, ct.ToGrainCancellationToken());

		await grain.UpdateRaffleAsync(model, ct.ToGrainCancellationToken());

		//this.orleansClient.ProcessEvent<IUpdateWeb3RaffleEvent, Web3RaffleModel>(req.ConnectionId, model, ct);

		await this.SendOkAsync(model.ToResponseModel(), ct);
	}
}