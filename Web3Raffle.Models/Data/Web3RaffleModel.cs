using Web3raffle.Models.Enums;
using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public class Web3RaffleModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[Display(Name = "Project Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string ProjectId { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public string Name { get; set; } = string.Empty;

		[DataType(DataType.DateTime)]
		[Display(Name = "Start Date")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;

		[DataType(DataType.DateTime)]
		[Display(Name = "End Date")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public DateTimeOffset? EndDate { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(4)]
		public string? Description { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Enable Max Entrant")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(5)]
		public bool EnableMaxEntrant { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Max Entrant")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(6)]
		public int? MaxEntrant { get; set; } = 0;

		[DataType(DataType.Custom)]
		[Display(Name = "Limit Count")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(7)]
		public int LimitCount { get; set; } = 1;

		[DataType(DataType.Custom)]
		[Display(Name = "Allow Duplicate Winner")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(8)]
		public bool AllowDuplicateWinner { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Disable Until Start Date")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(9)]
		public bool DisableUntilStartDate { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Auto Run Raffle")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(10)]
		public bool AutoRunRaffle { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Custom Randomizer Seed")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(11)]
		public bool CustomRandomizerSeed { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Randomize Seed")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(12)]
		public int RandomizeSeed { get; set; } = 0;

		[DataType(DataType.Custom)]
		[Display(Name = "Required Phone Number")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(13)]
		public bool RequiredPhoneNumber { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Required Email")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(14)]
		public bool RequiredEmail { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Password Protect")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(15)]
		public bool PasswordProtect { get; set; } = false;

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(16)]
		public string? Password { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Enable Black List")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(17)]
		public bool EnableBlackList { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Enable White List")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(18)]
		public bool EnableWhiteList { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Enable Webhook")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(19)]
		public bool? EnableWebhook { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Webhook On Entered")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(20)]
		public bool WebhookOnEntered { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Webhook On Done")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(21)]
		public bool WebhookOnDone { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Webhook On Started")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(22)]
		public bool WebhookOnStarted { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Webhook Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(23)]
		public string? WebhookUrl { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Icon Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(24)]
		public string? RaffleIconUrl { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Banner Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(25)]
		public string? RaffleBannerUrl { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Created By")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(26)]
		public string CreatedBy { get; set; } = "SYSTEM";

		[DataType(DataType.Custom)]
		[Display(Name = "Winning Selection Count")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(27)]
		public int WinningSelectionCount { get; set; } = 1;

		[DataType(DataType.DateTime)]
		[Display(Name = "Raffle Drawn Date")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(28)]
		public DateTimeOffset? RaffleDrawnDate { get; set; }

		[DataType(DataType.Custom)]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(29)]
		public RaffleStatus Status { get; set; } = RaffleStatus.Initial;

		[DataType(DataType.Custom)]
		[Display(Name = "Number of Entrants")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(30)]
		public int NumberOfEntrants { get; set; } = 0;

		// PRIVATE RAFFLE ENTER BY ADMIN
		[DataType(DataType.Custom)]
		[Display(Name = "Disable Public Entrance")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(31)]
		public bool DisablePublicEntrance { get; set; } = false;

		[DataType(DataType.Custom)]
		[Display(Name = "Overview")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(32)]
		public string? Overview { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "White List Disclaimer")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(33)]
		public string? WhiteListDisclaimer { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Black List Disclaimer")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(34)]
		public string? BlackListDisclaimer { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "External URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(35)]
		public string? ExternalURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Terms Of Use External Url")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(36)]
		public string? TermsOfUseExternalUrl { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Privacy Policy External URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(37)]
		public string? PrivacyPolicyExternalURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Primary Color")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(38)]
		public string? RafflePrimaryColor { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Secondary Color")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(39)]
		public string? RaffleSecondaryColor { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Logo URL")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(40)]
		public string? RaffleLogoURL { get; set; }

		[DataType(DataType.Custom)]
		[Display(Name = "URL Slug")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(41)]
		public string UrlSlug { get; set; } = string.Empty;
	}
}