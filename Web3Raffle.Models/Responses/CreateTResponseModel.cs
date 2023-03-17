namespace Web3raffle.Models.Responses
{
	[GenerateSerializer]
	public class CreateTResponseModel<T> : BaseResponseDataModel
	{
		public CreateTResponseModel()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public CreateTResponseModel(T data)
		{
			this.Id = Guid.NewGuid().ToString();
			this.Data = data;
		}

		[Id(0)]
		public T? Data { get; set; }
	}
}