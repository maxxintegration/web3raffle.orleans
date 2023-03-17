namespace Web3raffle.Models.Requests
{
	[GenerateSerializer]
	public class QueryModel
	{
		[BindFrom("$top")]
		[Display(Name = "Top")]
		[JsonPropertyName("$top")]
		[DataType(DataType.Text)]
		[Id(0)]
		public int? Top { get; set; } = 100000;

		[BindFrom("$skip")]
		[Display(Name = "Skip")]
		[JsonPropertyName("$skip")]
		[DataType(DataType.Text)]
		[Id(1)]
		public int? Skip { get; set; } = 0;

		[BindFrom("$filter")]
		[Display(Name = "Filter")]
		[JsonPropertyName("$filter")]
		[DataType(DataType.Text)]
		[Id(2)]
		public string? Filter { get; set; }

		[BindFrom("$orderby")]
		[Display(Name = "OrderBy")]
		[JsonPropertyName("$orderby")]
		[DataType(DataType.Text)]
		[Id(3)]
		public string? OrderBy { get; set; }

		[BindFrom("$search")]
		[Display(Name = "Search")]
		[JsonPropertyName("$search")]
		[DataType(DataType.Text)]
		[Id(4)]
		public string? Search { get; set; }

		[BindFrom("$select")]
		[Display(Name = "Select")]
		[JsonPropertyName("$select")]
		[DataType(DataType.Text)]
		[Id(5)]
		public string? Select { get; set; }

		[BindFrom("$facet")]
		[Display(Name = "Facets")]
		[JsonPropertyName("$facet")]
		[DataType(DataType.Text)]
		[Id(6)]
		public string? Facets { get; set; }

		public void AppendFilter(string filter)
		{
			this.Filter += $" and {filter}";
			this.Filter = this.Filter.Trim();

			if (this.Filter.StartsWith("and"))
			{
				this.Filter = this.Filter.Substring(4, this.Filter.Length - 4);
			}
		}
	}
}