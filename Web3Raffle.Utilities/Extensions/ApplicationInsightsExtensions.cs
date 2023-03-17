using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Reflection;

namespace Web3raffle.Utilities.Extensions
{
	public static class ApplicationInsightsExtensions
	{
		public static void AddWebAppApplicationInsights(this IServiceCollection services)
		{
			var apiName = Assembly.GetCallingAssembly().GetName().Name ?? "VeeFriends";

			services.AddApplicationInsightsTelemetry();
			services.AddSingleton<ITelemetryInitializer>((services) => new ApplicationMapNodeNameInitializer(apiName));
		}
	}

	internal class ApplicationMapNodeNameInitializer : ITelemetryInitializer
	{
		public string Name { get; set; }

		public ApplicationMapNodeNameInitializer(string name)
		{
			this.Name = name;
		}

		public void Initialize(ITelemetry telemetry)
		{
			telemetry.Context.Cloud.RoleName = this.Name;
		}
	}
}