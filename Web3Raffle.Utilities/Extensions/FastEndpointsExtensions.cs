using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Orleans;
using System.Reflection;

namespace Web3raffle.Utilities.Extensions
{
	public static class FastEndpointsExtensions
	{
		public static IServiceCollection ConfigureFastEndpoints(this IServiceCollection builder, IConfiguration configuration)
		{
			var apiName = Assembly.GetCallingAssembly().GetName().Name;

			builder.AddFastEndpoints();
			builder.AddCorsOptions();
			builder.AddSwaggerDoc(
				shortSchemaNames: true,
				maxEndpointVersion: 1,
				serializerSettings: x =>
				{
					x.MergeDefaultJsonOptions();
				},
				settings: settings =>
				{
					settings.DocumentName = apiName;
					settings.Title = apiName;
					settings.Version = "v1.0";
					settings.EnableJWTBearerAuth();
				});


			return builder;
		}

		public static WebApplication ConfigureFastEndpoints(this WebApplication app)
		{
			var apiName = Assembly.GetCallingAssembly().GetName().Name;

			app.UseRouting();
			app.UseCors();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseFastEndpoints(c =>
			{
				c.Versioning.Prefix = "v";
				c.Versioning.DefaultVersion = 1;
				c.Versioning.PrependToRoute = true;

				c.Serializer.MergeDefaultJsonOptions();

				c.Endpoints.ShortNames = true;

				//c.Errors.ResponseBuilder = (errors, context, statusCode) =>
				//{
				//	var errorResponse = ExceptionHandlerExtensions
				//		.CreateErrorModel(errors);

				//	context.Response.ContentType = "application/json";
				//	context.Response.StatusCode = errorResponse.StatusCode;

				//	return errorResponse;
				//};
			});

			app.UseOpenApi();
			app.UseSwaggerUi3(c => c.ConfigureDefaults());

			app.MapGet("/", (CancellationToken cancellationToken) =>
			{
				return Results.Ok(apiName);
			}).ExcludeFromDescription();

			app.UseForwardedHeaders();
			app.UseResponseCaching();

			return app;
		}

		public static IServiceCollection AddCorsOptions(this IServiceCollection service)
		{
			service
				.AddCors(options =>
					options.AddDefaultPolicy(
						builder =>
						{
							builder.SetIsOriginAllowed((host) => true);
							builder.AllowAnyMethod();
							builder.AllowAnyHeader();
							builder.AllowCredentials();
						}));

			service
				.Configure<ForwardedHeadersOptions>(options =>
				{
					options.ForwardedHeaders =
						ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
				});

			return service;
		}
	}
}