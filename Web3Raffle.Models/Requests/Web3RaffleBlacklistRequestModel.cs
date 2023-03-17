using Web3raffle.Models.Data;

namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class Web3RaffleBlacklistRequestModel
	{
		public Web3RaffleBlacklistRequestModel()
		{
			this.Data = new List<Web3RaffleBlacklistModel>();
		}

		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[Id(1)]
		public string WalletAddress { get; set; } = string.Empty;

		[Id(2)]
		public string? ConnectionId { get; set; }

		[Id(3)]
		public List<Web3RaffleBlacklistModel> Data { get; set; }
	}
}