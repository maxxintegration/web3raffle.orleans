using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleWinnerModel : BaseResponseDataModel
	{
		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[Id(1)]
		public string RaffleName { get; set; } = string.Empty;

		[Id(2)]
		public string WalletAddress { get; set; } = string.Empty;

		[Id(3)]
		public string? DisplayName { get; set; }

		[Id(4)]
		public string? Email { get; set; }

	}
}