namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class RaffleQueryModel : QueryModel
	{
		[Id(0)]
		public string? RaffleId { get; init; } = "0";

		[Id(1)]
		public string? EntrantId { get; init; } = "0";

		[Id(2)]
		public string? WalletAddress { get; init; } = "0";
	}
}