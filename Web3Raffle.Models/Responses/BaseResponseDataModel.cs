namespace Web3raffle.Models.Responses
{
	[GenerateSerializer]
	public abstract class BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("id")]
		[Key]
		[Display(Name = "ID")]
		[SimpleField(IsKey = true, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(0)]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[DataType(DataType.Text)]
		[JsonPropertyName("tags")]
		[Display(Name = "Keywords")]
		[SimpleField(IsKey = false, IsFilterable = true, IsFacetable = true)]
		[Id(2)]
		public List<string>? Tags { get; set; }

		[DataType(DataType.DateTime)]
		[JsonPropertyName("createdAt")]
		[Display(Name = "Created At")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(3)]
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

		[DataType(DataType.DateTime)]
		[JsonPropertyName("modifiedAt")]
		[Display(Name = "Modified At")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(4)]
		public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;

		[DataType(DataType.DateTime)]
		[JsonPropertyName("deletedAt")]
		[Display(Name = "Deleted At")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(5)]
		public DateTimeOffset? DeletedAt { get; set; }

		[DataType(DataType.Custom)]
		[JsonPropertyName("deleted")]
		[Display(Name = "Deleted")]
		[SimpleField(IsKey = false, IsFilterable = true, IsSortable = true, IsFacetable = true)]
		[Id(6)]
		public bool? Deleted { get; set; } = false;
	}
}