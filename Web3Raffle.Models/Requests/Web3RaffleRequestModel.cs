using Web3raffle.Models.Data;

namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class Web3RaffleRequestModel : Web3RaffleModel
	{
		[DataType(DataType.Custom)]
		[Display(Name = "Connection Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string? ConnectionId { get; set; }
	}
}