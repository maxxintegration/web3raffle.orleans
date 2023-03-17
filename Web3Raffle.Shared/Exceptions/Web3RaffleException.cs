namespace Web3raffle.Shared.Exceptions
{
	[GenerateSerializer]
	public class Web3RaffleException : Exception
	{
		public Web3RaffleException()
		{ }

		public Web3RaffleException(string message) : base(message)
		{
		}

		public Web3RaffleException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected Web3RaffleException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}