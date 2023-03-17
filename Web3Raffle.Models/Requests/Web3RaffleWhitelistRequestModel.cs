using Web3raffle.Models.Data;

namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class Web3RaffleWhitelistRequestModel
	{
		public Web3RaffleWhitelistRequestModel()
		{
			this.Data = new List<Web3RaffleWhitelistModel>();
		}

		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[Id(1)]
		public string WalletAddress { get; set; } = string.Empty;

		[Id(2)]
		public int LimitCount { get; set; } = 0;

		[Id(3)]
		public string? ConnectionId { get; set; }

		[Id(4)]
		public List<Web3RaffleWhitelistModel> Data { get; set; }
	}
}