namespace Web3raffle.Models.Responses
{
	[GenerateSerializer]
	public class LinkResponseModel
	{
		public LinkResponseModel()
		{
		}

		public LinkResponseModel(string href)
		{
			this.Href = new Uri(href);
		}

		[DataType(DataType.Text)]
		[JsonPropertyName("rel")]
		[Id(0)]
		public string Rel { get; set; } = "noreferrer nofollow";

		[DataType(DataType.Url)]
		[JsonPropertyName("href")]
		[Display(Name = "Reference URL")]
		[Url]
		[Id(1)]
		public Uri? Href { get; set; }

		[DataType(DataType.Text)]
		[JsonPropertyName("action")]
		[Id(2)]
		public HttpMethod Action { get; set; } = HttpMethod.Get;

		[DataType(DataType.Text)]
		[JsonPropertyName("types")]
		[Id(3)]
		public List<string>? Types { get; set; }
	}

	[GenerateSerializer]
	public struct HttpMethodSurrogate
	{
		[Id(0)]
		public string _method;
	}

	[RegisterConverter]
	public sealed class MyForeignLibraryValueTypeSurrogateConverter :
		IConverter<HttpMethod, HttpMethodSurrogate>
	{
		public HttpMethod ConvertFromSurrogate(
			in HttpMethodSurrogate surrogate) =>
			new(surrogate._method);

		public HttpMethodSurrogate ConvertToSurrogate(
			in HttpMethod value) =>
			new()
			{
				_method = value.Method
			};
	}
}