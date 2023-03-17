namespace Web3raffle.Models.Responses
{
	[GenerateSerializer]
	public class ResponseModel<T> where T : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("data")]
		[Display(Name = "Data")]
		[Id(0)]
		public T Data { get; set; } = default!;

		[DataType(DataType.Custom)]
		[JsonPropertyName("links")]
		[Display(Name = "Links")]
		[Id(1)]
		public List<LinkResponseModel> Links { get; private set; } = new List<LinkResponseModel>();
	}

	[GenerateSerializer]
	public class ErrorModel : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("statusCode")]
		[Display(Name = "Status Code")]
		[Id(0)]
		public int StatusCode { get; set; } = default!;

		[DataType(DataType.Custom)]
		[JsonPropertyName("stackTrace")]
		[Display(Name = "Stack Trace")]
		[Id(1)]
		public string? StackTrace { get; set; }

		[DataType(DataType.Custom)]
		[JsonPropertyName("messages")]
		[Display(Name = "Messages")]
		[Id(2)]
		public List<string>? Messages { get; set; }
	}
}