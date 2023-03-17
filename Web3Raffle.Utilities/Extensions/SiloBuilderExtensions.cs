using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Serialization;
using Orleans.Serialization.Cloning;
using Orleans.Serialization.Serializers;
using System.Linq.Expressions;
using Serilog;
using Web3raffle.Utilities.Serializers;

namespace Web3raffle.Utilities.Extensions
{
	public static class SiloBuilderExtensions
	{
		public static ISiloBuilder ConfigureOrleans(this ISiloBuilder siloBuilder, IConfiguration configuration, (string SiloName, int SiloPort, int GatewayPort, bool isDashboard)? siloOptions = null)
		{
			var azureStorageConnectionString = configuration.GetConnectionStringOrDefault("AZURE_STORAGE_CONNECTION_STRING");
			var clusterId = configuration.GetConnectionStringOrDefault("Orleans:ClusterId");
			var serviceId = configuration.GetConnectionStringOrDefault("Orleans:ServiceId");

			var siloName = siloOptions?.SiloName ?? "Silo";
			var siloPort = siloOptions?.SiloPort ?? 11_111;
			var gatewayPort = siloOptions?.GatewayPort ?? 30_000;
			var isDashboard = siloOptions?.isDashboard ?? false;

			siloBuilder.Services.AddSerializer(serializerBuilder =>
			{
				serializerBuilder.AddExpressionTypeSerializer();
			});

			siloBuilder
				.Configure<ClusterOptions>(options =>
				{
					options.ClusterId = clusterId;
					options.ServiceId = serviceId;
				})
				.Configure<SiloOptions>(options =>
				{
					options.SiloName = siloName;
				})
				.ConfigureEndpoints(
					siloPort: siloPort,
					gatewayPort: gatewayPort
				);

			if (isDashboard)
			{
				siloBuilder.UseDashboard(config =>
					config.HideTrace = true
				);
			}

			if (string.IsNullOrEmpty(azureStorageConnectionString))
			{
				siloBuilder.UseLocalhostClustering();
				siloBuilder.AddMemoryGrainStorage("Dev");
			}
			else
			{
				siloBuilder
					.UseAzureStorageClustering(options => options.ConfigureTableServiceClient(azureStorageConnectionString))
					.AddAzureTableGrainStorage("AzureTableGrainStorage", (options) =>
					{
						options.ConfigureTableServiceClient(azureStorageConnectionString);
					});
			}

			return siloBuilder;
		}

		public static IHostBuilder ConnectOrleansClient(this IHostBuilder host, IConfiguration configuration)
		{
			return host.UseOrleansClient(builder =>
			{
				var azureStorageConnectionString = configuration
					.GetConnectionStringOrDefault("AZURE_STORAGE_CONNECTION_STRING");

				builder.Services.AddSerializer(serializerBuilder =>
				{
					serializerBuilder.AddExpressionTypeSerializer();
				});

				builder.Configure<ClusterOptions>(options =>
				{
					options.ClusterId = configuration.GetConnectionStringOrDefault("Orleans:ClusterId");
					options.ServiceId = configuration.GetConnectionStringOrDefault("Orleans:ServiceId");
				});

				if (string.IsNullOrEmpty(azureStorageConnectionString))
				{
					builder.UseLocalhostClustering();
				}
				else
				{
					builder.UseAzureStorageClustering(options =>
					{
						options.ConfigureTableServiceClient(azureStorageConnectionString);
					});
				}
			});
		}

		public static IHostBuilder AddSerilog(this IHostBuilder host, IConfiguration configuration, string appName)
		{
			return host.UseSerilog((ctx, loggerConfiguration) =>
			{
				loggerConfiguration
					.Enrich.WithProperty("App", appName)
					.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {App} {ThreadId}{NewLine}{Exception}")
					.ReadFrom.Configuration(ctx.Configuration);

				var env = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

				if (env == "Local")
				{
					loggerConfiguration.WriteTo.Seq("http://localhost:5341");
				}
				else
				{
					loggerConfiguration.WriteTo.Seq(configuration["Seq:BaseUrl"]!, apiKey: configuration["Seq:ApiKey"]!);
				}

			});
		}
	}

	public static class SerializationHostingExtensions
	{
		private static readonly ServiceDescriptor ExpressionServiceDescriptor = new(typeof(ExpressionTypeSerializer), typeof(ExpressionTypeSerializer));

		public static ISerializerBuilder AddExpressionTypeSerializer(
			this ISerializerBuilder builder)
		{
			var services = builder.Services;

			if (services.Contains(ExpressionServiceDescriptor))
			{
				return builder;
			}

			services.AddSingleton<ICodecSelector>(new DelegateCodecSelector
			{
				CodecName = nameof(ExpressionTypeSerializer),
				IsSupportedTypeDelegate = (x) => IsSupportedType(x)
			});

			services.AddSingleton<ICopierSelector>(new DelegateCopierSelector
			{
				CopierName = nameof(ExpressionTypeSerializer),
				IsSupportedTypeDelegate = (x) => IsSupportedType(x)
			});

			services.AddSingleton<ExpressionTypeSerializer>();
			services.AddSingleton<IGeneralizedCodec, ExpressionTypeSerializer>();
			services.AddSingleton<IGeneralizedCopier, ExpressionTypeSerializer>();

			builder.Configure(options => options.WellKnownTypeAliases[nameof(ExpressionTypeSerializer)] = typeof(ExpressionTypeSerializer));

			return builder;

			static bool IsSupportedType(Type itemType)
			{
				var isExpression = itemType == typeof(Expression);
				var isExpressionAssigned = itemType.IsAssignableFrom(typeof(Expression));
				var isExpressionSubclassed = itemType.IsSubclassOf(typeof(Expression));

				if (isExpression || isExpressionAssigned || isExpressionSubclassed)
				{
					return true;
				}

				return false;
			}
		}
	}
}