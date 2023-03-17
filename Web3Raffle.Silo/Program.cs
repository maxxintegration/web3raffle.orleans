using Web3raffle.Silo.Extensions;
using Serilog;
using Web3raffle.Data.Hubs;
using Web3raffle.Utilities.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
	.Host
	.AddConfig(configuration)
	.AddSerilog(configuration, AppDomain.CurrentDomain.FriendlyName);

builder.Host.UseOrleans(siloBuilder =>
{
	siloBuilder
			.ConfigureOrleans(configuration)
			.ConfigureServices(service =>
			{
				service
					.AddCosmosDb()
					.AddCosmosClientWeb3Raffle()
					.AddCorsOptions()
					.AddWebAppApplicationInsights();
			});
});

var app = builder.Build();

app
	.UseSerilogRequestLogging()
	.UseCors()
	.UseRouting()
	.UseEndpoints(x =>
	{
		x.MapHub<MessageHub>("/message");
	});

app
	.MapGet("/", (CancellationToken cancellationToken) =>
	{
		return Results.Ok("Silo is running!");
	});

app.Run();