using Web3raffle.Models.Data;

namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class Web3RaffleEntrantRequestModel
	{
		public Web3RaffleEntrantRequestModel()
		{
			this.Data = new List<Web3RaffleEntrantModel>();
		}

		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[Id(1)]
		public string? ConnectionId { get; set; }

		[Id(2)]
		public string? Password { get; set; }

		[Id(3)]
		public List<Web3RaffleEntrantModel> Data { get; set; }
	}
}