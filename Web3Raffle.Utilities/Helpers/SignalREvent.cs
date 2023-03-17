using System.Text;

namespace Web3raffle.Utilities.Helpers
{
	[GenerateSerializer]
	public record SignalREvent<T>(string ProcessName, string RecipientId, Guid GrainKey, T? Payload = null, string? Error = null, Dictionary<string, object?>? AdditionalData = null) where T : class
	{
		[Id(0)]
		public string InvocationType { private set; get; } = "Unknown";

		[Id(1)]
		public string? ConnectionId { private set; get; }

		[Id(2)]
		public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

		[Id(3)]
		public bool IsSuccess { get; } = Error is null;

		[Id(4)]
		public bool HasPayload { get; } = Payload is not null;

		[Id(5)]
		public bool HasAdditionalData { get; } = AdditionalData is not null and { Count: > 0 };

		public void SetInvocationType(string invocationType)
		{
			this.InvocationType = invocationType;
		}

		public void SetConnectionId(string? connectionId)
		{
			if (string.IsNullOrEmpty(connectionId))
			{
				return;
			}

			this.ConnectionId = connectionId;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb
				.Append($"[{this.ProcessName}]")
				.Append($"[{this.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff")}]: \n")
				.AppendLine($"  -> Recipient: {this.RecipientId}")
				.AppendLine($"  -> GrainKey: {this.GrainKey}")
				.AppendLine($"  -> Payload: {this.Payload}")
				.AppendLine($"  -> Error: {this.Error}")
				.AppendLine($"  -> InvocationType: {this.InvocationType}")
				.AppendLine($"  -> ConnectionId: {this.ConnectionId}")
				.AppendLine($"  -> IsSuccess: {this.IsSuccess}")
				.AppendLine($"  -> HasPayload: {this.HasPayload}")
				.AppendLine($"  -> HasAdditionalData: {this.HasAdditionalData}");

			return sb.ToString();
		}
	}

	public record SignalRListEvent<T>(string ProcessName, string RecipientId, Guid GrainKey, List<T>? Payload = null, string? Error = null, Dictionary<string, object>? AdditionalData = null) where T : class
	{
		public string InvocationType { private set; get; } = "Unknown";

		public string? ConnectionId { private set; get; }

		public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

		public bool IsSuccess { get; } = Error is null;

		public bool HasPayload { get; } = Payload is not null;

		public bool HasAdditionalData { get; } = AdditionalData is not null and { Count: > 0 };

		public void SetInvocationType(string invocationType)
		{
			this.InvocationType = invocationType;
		}

		public void SetConnectionId(string? connectionId)
		{
			if (string.IsNullOrEmpty(connectionId))
			{
				return;
			}

			this.ConnectionId = connectionId;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb
				.Append($"[{this.ProcessName}]")
				.Append($"[{this.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff")}]: \n")
				.AppendLine($"  -> Recipient: {this.RecipientId}")
				.AppendLine($"  -> GrainKey: {this.GrainKey}")
				.AppendLine($"  -> Payload: {this.Payload}")
				.AppendLine($"  -> Error: {this.Error}")
				.AppendLine($"  -> InvocationType: {this.InvocationType}")
				.AppendLine($"  -> ConnectionId: {this.ConnectionId}")
				.AppendLine($"  -> IsSuccess: {this.IsSuccess}")
				.AppendLine($"  -> HasPayload: {this.HasPayload}")
				.AppendLine($"  -> HasAdditionalData: {this.HasAdditionalData}");

			return sb.ToString();
		}
	}
}