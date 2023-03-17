using Web3raffle.Models.Responses;

namespace Web3raffle.Shared.Exceptions
{
	[GenerateSerializer]
	public class CosmosDbException : Exception
	{
		public CosmosDbException()
		{
		}

		public CosmosDbException(string? message) : base(message)
		{
		}

		public CosmosDbException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected CosmosDbException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public static void ThrowBadModel<T>(string message, T modelToCreate) where T : BaseResponseDataModel
		{
			var json = System.Text.Json.JsonSerializer.Serialize(modelToCreate);

			throw new CosmosDbException($"{message}: {json}");
		}

		public static void ThrowEmptyModel<T>(T? modelToCreate) where T : BaseResponseDataModel
		{
			if (modelToCreate is null or { Id: null })
			{
				throw new CosmosDbException("The model returned was empty or null");
			}
		}

		public static void ThrowEmptyModel<T>(List<T>? modelToCreate) where T : BaseResponseDataModel
		{
			if (modelToCreate is null or { Count: 0 })
			{
				throw new CosmosDbException("The models returned were empty or null");
			}
		}
	}
}