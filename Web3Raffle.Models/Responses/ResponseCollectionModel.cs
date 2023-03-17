namespace Web3raffle.Models.Responses
{
	[GenerateSerializer]
	public class ResponseCollectionModel<T> : ResponseModel<T> where T : BaseResponseDataModel
	{
		[DataType(DataType.Custom)]
		[JsonPropertyName("data")]
		[Display(Name = "Data")]
		[Id(0)]
		public new List<T> Data { get; set; } = default!;

		[DataType(DataType.Text)]
		[JsonPropertyName("length")]
		[Display(Name = "Length")]
		[Id(1)]
		public int? Length { get; set; } = default!;

		//public int? Length
		//{
		//	get
		//	{
		//		if (this._overiddenLength is not null)
		//		{
		//			return this._overiddenLength.Value;
		//		}

		//		if (this.Data is null)
		//		{
		//			return null;
		//		}

		//		return this.Data.Count;
		//	}
		//}

		//[JsonIgnore]
		//private int? _overiddenLength;

		public void SetLength(int length)
		{
			//this._overiddenLength = length;
			this.Length = length;
		}

		public int? GetLength()
		{
			if (this.Length is not null)
			{
				return this.Length.Value;
			}

			if (this.Data is null)
			{
				return null;
			}

			return this.Data.Count;
		}
	}
}