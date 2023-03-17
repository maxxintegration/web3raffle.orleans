using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public class Web3RaffleProjectModel : BaseResponseDataModel
	{
		[Required]
		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public string Description { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string? Logo { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public string? Twitter { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(4)]
		public string? Discord { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "External URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(5)]
		public string? ExternalURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Terms of Use External Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(6)]
		public string? TermsOfUseExternalUrl { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Privacy Policy External URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(7)]
		public string? PrivacyPolicyExternalURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Primary Color")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(8)]
		public string? RafflePrimaryColor { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Secondary Color")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(9)]
		public string? RaffleSecondaryColor { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Logo URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(10)]
		public string? RaffleLogoURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Url Slug")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(11)]
		public string UrlSlug { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Created By")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(12)]
		public string? CreatedBy { get; set; } = string.Empty;
	}
}