using Microsoft.AspNetCore.SignalR;

namespace Web3raffle.Data.Hubs;

public class MessageHub : Hub
{
	public string GetConnectionId() => this.Context.ConnectionId;
}