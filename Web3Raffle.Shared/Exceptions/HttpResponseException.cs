namespace Web3raffle.Shared.Exceptions
{
	[GenerateSerializer]
	public class HttpResponseException : Exception
	{
		public HttpResponseException()
		{ }

		public HttpResponseException(string message) : base(message)
		{
		}

		public HttpResponseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}