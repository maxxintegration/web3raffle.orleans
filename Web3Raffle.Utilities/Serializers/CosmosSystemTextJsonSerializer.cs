using Azure.Core.Serialization;

namespace Web3raffle.Utilities.Serializers;

public sealed class CosmosSystemTextJsonSerializer : CosmosSerializer
{
	private readonly JsonSerializerOptions jsonSerializerOptions;

	public CosmosSystemTextJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
	{
		this.jsonSerializerOptions = jsonSerializerOptions;
	}

	public override T FromStream<T>(Stream stream)
	{
		if (stream.CanSeek && stream.Length == 0)
		{
			return default!;
		}

		if (typeof(Stream).IsAssignableFrom(typeof(T)))
		{
			return (T)(object)stream;
		}

		var systemTextJsonSerializer = this.GetSerializer();

		using (stream)
		{
			var result = systemTextJsonSerializer
				.Deserialize(stream, typeof(T), default)!;

			return (T)result;
		}
	}

	public override Stream ToStream<T>(T input)
	{
		var streamPayload = new MemoryStream();
		var systemTextJsonSerializer = this.GetSerializer();

		systemTextJsonSerializer
			.Serialize(streamPayload, input, typeof(T), default);

		streamPayload.Position = 0;

		return streamPayload;
	}

	private JsonObjectSerializer GetSerializer()
	{
		return new JsonObjectSerializer(this.jsonSerializerOptions);
	}
}