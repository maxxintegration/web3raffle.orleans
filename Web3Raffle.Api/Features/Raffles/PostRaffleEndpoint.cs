using Web3raffle.Shared.Exceptions;
using Web3raffle.Abstractions;
using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Abstractions.ProcessEventInterfaces;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Raffles;

public class PostRaffleEndpoint : Endpoint<Web3RaffleRequestModel, ResponseModel<Web3RaffleModel>>
{
	private readonly IClusterClient orleansClient;

	public PostRaffleEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Post("/raffles");
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

		await grain.ValidateRaffleAsync(model, true, ct.ToGrainCancellationToken());

		await this.orleansClient
			.ProcessEvent<ICreateWeb3RaffleEvent, Web3RaffleModel>(req.ConnectionId, model, ct.ToGrainCancellationToken());

		await this.SendOkAsync(model.ToResponseModel(), ct);
	}
}