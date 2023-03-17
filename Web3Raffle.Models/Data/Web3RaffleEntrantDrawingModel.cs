namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleEntrantDrawingModel
	{
		[Id(0)]
		public string WalletAddress { get; set; } = string.Empty;

		[Id(1)]
		public string HashBytes { get; set; } = string.Empty;
	}
}