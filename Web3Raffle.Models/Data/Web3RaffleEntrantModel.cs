using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleEntrantModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Wallet Address")]
		[Required(ErrorMessage = "Wallet Address is required field")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public string WalletAddress { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Display Name")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string? DisplayName { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Avatar Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public string? AvatarUrl { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(4)]
		public string? Email { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(5)]
		public string? Phone { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(6)]
		public string? Twitter { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(7)]
		public string? Discord { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Entrant Sequence")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(8)]
		public int EntrantSequence { get; set; } = 0;

		[Id(9)]
		public bool IsWinner { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Created By")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(10)]
		public string? CreatedBy { get; set; } = string.Empty;

		[JsonIgnore]
		[Id(11)]
		public string? RafflePassword { get; set; } = string.Empty;
	}
}