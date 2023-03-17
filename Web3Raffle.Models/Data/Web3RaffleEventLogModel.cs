using Web3raffle.Models.Enums;
using Web3raffle.Models.Responses;

namespace Web3raffle.Models.Data
{
	[GenerateSerializer]
	public sealed class Web3RaffleEventLogModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[Display(Name = "Raffle Id")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string RaffleId { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Event Type")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(1)]
		public EventLogType LogType { get; set; } = EventLogType.Project;

		[DataType(DataType.Custom)]
		[Display(Name = "Log Message")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(2)]
		public string? LogMessage { get; set; } = string.Empty;

		[DataType(DataType.Custom)]
		[Display(Name = "Created By")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public string? CreatedBy { get; set; } = string.Empty;
	}
}