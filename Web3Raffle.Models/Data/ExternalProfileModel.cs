namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class ExternalProfileModel
	{
		[JsonPropertyName("avatarUrl")]
		[Id(0)]
		public string? AvatarUrl { get; set; }

		[JsonPropertyName("name")]
		[Id(1)]
		public string? Name { get; set; }
	}
}