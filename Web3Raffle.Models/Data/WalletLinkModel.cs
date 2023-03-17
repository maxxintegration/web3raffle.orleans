using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class WalletLinkModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("status")]
		[Display(Name = "Status")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public WalletLinkStatus Status { get; set; } = WalletLinkStatus.Pending;

		[DataType(DataType.Custom)]
		[JsonPropertyName("userId")]
		[Display(Name = "User Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public string? UserId { get; set; }

		[DataType(DataType.Currency)]
		[JsonPropertyName("value")]
		[Display(Name = "Value")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public decimal? Value { get; set; }

		[DataType(DataType.Custom)]
		[JsonPropertyName("startinBlock")]
		[Display(Name = "Starting Block")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public string? StartingBlock { get; set; } = "0x0";

		[DataType(DataType.Custom)]
		[JsonPropertyName("verifyAddress")]
		[Display(Name = "Verify Address")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(4)]
		public string? VerifyAddress { get; set; }

		[DataType(DataType.Custom)]
		[JsonPropertyName("linkedAddress")]
		[Display(Name = "Linked Address")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(5)]
		public string? LinkedAddress { get; set; }

		[DataType(DataType.Custom)]
		[JsonPropertyName("transactionHash")]
		[Display(Name = "TransactionHash")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(6)]
		public string? TransactionHash { get; set; }
	}

	public enum WalletLinkStatus
	{
		[Display(Name = "pending")]
		Pending,

		[Display(Name = "verified")]
		Verified,

		[Display(Name = "expired")]
		Expired
	}
}