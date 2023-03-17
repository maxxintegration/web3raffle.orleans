namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class ProcessFailureModel
	{
		[JsonPropertyName("retries")]
		[Display(Name = "Retries")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public int Retries { get; set; } = 0;

		[JsonPropertyName("lastAttempt")]
		[Display(Name = "Last Attempt")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public DateTimeOffset? LastAttempt { get; set; }

		[JsonPropertyName("error")]
		[Display(Name = "Error")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string LastMessage { get; set; } = string.Empty;
	}
}