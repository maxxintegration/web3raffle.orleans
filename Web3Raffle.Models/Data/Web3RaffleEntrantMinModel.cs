using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleEntrantMinModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("walletAddress")]
		[Display(Name = "Wallet Address")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string WalletAddress { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[JsonPropertyName("amount")]
		[Display(Name = "Amount")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public int Amount { get; set; } = 0;
	}
}