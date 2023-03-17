using System.Globalization;
using Web3raffle.Utilities.Serializers;
using Web3raffle.Models.Responses;

namespace Web3raffle.Utilities.Extensions;

public static class ServiceCollectionExtensions
{

	public static ConfigureHostBuilder AddConfig(this ConfigureHostBuilder host, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionStringOrDefault("AZURE_APP_CONFIG_CONNECTION_STRING");

		host.ConfigureHostConfiguration(builder =>
		{
			builder.AddAzureAppConfiguration(options =>
			{
				options.Connect(connectionString);
				options.UseFeatureFlags();
			});
		});

		return host;
	}

	public static IServiceCollection AddCosmosDb(this IServiceCollection services)
	{
		services.AddSingleton(x =>
		{
			var configuration = x.GetRequiredService<IConfiguration>();
			var connectionString = configuration.GetConnectionStringOrDefault("AZURE_COMOSDB_CONNECTION_STRING");

			var documentClient = new CosmosClient(
				connectionString: connectionString,
				clientOptions: new CosmosClientOptions
				{
					AllowBulkExecution = true,
					ApplicationName = "VeeFriends",
					Serializer = new CosmosSystemTextJsonSerializer(JsonExtensions.DefaultOptions)
				}
			);

			return documentClient;
		});

		return services;
	}

	public static string GetConnectionStringOrDefault(this IConfiguration configuration, string connectionStringName)
	{
		var connectionString = configuration.GetConnectionString(connectionStringName);

		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = Environment.GetEnvironmentVariable(connectionStringName);
		}

		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = configuration[connectionStringName];
		}

		ArgumentNullException.ThrowIfNull(connectionString);

		return connectionString.Trim();
	}

	public static IServiceCollection AddCosmosClient<T>(this IServiceCollection services) where T : BaseResponseDataModel
	{
		services.AddSingleton(x =>
		{
			var configuration = x.GetRequiredService<IConfiguration>();
			var cosmosClient = x.GetRequiredService<CosmosClient>();

			var database = cosmosClient.GetDatabase(configuration.GetConnectionStringOrDefault("CosmosDb:DatabaseName"));

			var container = database.GetContainer(typeof(T).ToIndexName());

			return new CosmosDbRepository<T>(container);
		});

		return services;
	}

	public static string ToIndexName(this Type type)
	{
		var indexName = $"{type.Name.ToLower(CultureInfo.CurrentCulture).Trim()}";

		return indexName;
	}


}