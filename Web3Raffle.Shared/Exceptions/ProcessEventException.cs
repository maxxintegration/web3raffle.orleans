namespace Web3raffle.Shared.Exceptions
{
	[GenerateSerializer]
	public class ProcessEventException : Exception
	{
		public ProcessEventException()
		{
		}

		public ProcessEventException(string? message) : base(message)
		{
		}

		public ProcessEventException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected ProcessEventException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}