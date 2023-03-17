namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class IdRequestModel
	{
		[BindFrom("id")]
		[JsonPropertyName("id")]
		[Display(Name = "ID")]
		[Id(0)]
		public string? Id { get; set; }

		[BindFrom("raffleId")]
		[JsonPropertyName("raffleId")]
		[Display(Name = "Raffle Id")]
		[Id(1)]
		public string? RaffleId { get; set; }

		[BindFrom("connectionId")]
		[JsonPropertyName("connectionId")]
		[Display(Name = "Connection ID")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string? ConnectionId { get; set; }
	}
}