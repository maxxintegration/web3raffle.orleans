using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleBlacklistModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Wallet Address")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public string WalletAddress { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Created By")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string? CreatedBy { get; set; } = string.Empty;
	}
}