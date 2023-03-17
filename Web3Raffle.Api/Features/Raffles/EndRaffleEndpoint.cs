﻿using Web3raffle.Abstractions.GrainInterfaces;
using Web3raffle.Models.Requests;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Api.Features.Raffles;

public class EndRaffleEndpoint : Endpoint<RaffleQueryModel, EmptyResponse>
{
	private readonly IClusterClient orleansClient;

	public EndRaffleEndpoint(IClusterClient orleansClient)
	{
		this.orleansClient = orleansClient;
	}

	public override void Configure()
	{
		this.Delete("/raffles/{raffleId}/end");
		//this.AllowAnonymous();
		this.Roles("Admin");
		this.DontCatchExceptions();
	}

	public override async Task HandleAsync(RaffleQueryModel req, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(req);
		ArgumentNullException.ThrowIfNull(req.RaffleId);

		var primaryKey = Guid.Parse(req.RaffleId);

		var grain = this.orleansClient.GetGrain<IRaffleGrain>(primaryKey);

		await grain.EndRaffleAsync(req.RaffleId!, ct.ToGrainCancellationToken());

		await this.SendAsync(new EmptyResponse(), 200, ct);
	}
}