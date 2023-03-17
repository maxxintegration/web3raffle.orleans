namespace Web3raffle.Utilities.Extensions
{
	using Azure.Core.Serialization;
	using FastEndpoints;
	using System.Text.Json;
	using System.Text.Json.Serialization;

	public static class JsonExtensions
	{

		public static readonly JsonSerializerOptions DefaultOptions = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = false
		};

		public static readonly JsonObjectSerializer DefaultSearchSerializer = new(DefaultOptions);

		public static void MergeDefaultJsonOptions(this SerializerOptions options)
		{
			options.Options.PropertyNamingPolicy = DefaultOptions.PropertyNamingPolicy;
			options.Options.DefaultIgnoreCondition = DefaultOptions.DefaultIgnoreCondition;
			options.Options.WriteIndented = DefaultOptions.WriteIndented;
			options.Options.TypeInfoResolver = DefaultOptions.TypeInfoResolver;

			foreach (var property in DefaultOptions.Converters)
			{
				options.Options.Converters.Add(property);
			}
		}

		public static void MergeDefaultJsonOptions(this JsonSerializerOptions options)
		{
			options.PropertyNamingPolicy = DefaultOptions.PropertyNamingPolicy;
			options.DefaultIgnoreCondition = DefaultOptions.DefaultIgnoreCondition;
			options.WriteIndented = DefaultOptions.WriteIndented;

			foreach (var property in DefaultOptions.Converters)
			{
				options.Converters.Add(property);
			}
		}

	}
}