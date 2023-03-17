using Serilog;
using Web3raffle.Api.ExternalServices.Services;
using Web3raffle.Api.ExternalServices.IServices;
using Web3raffle.Utilities.Extensions;
using Web3raffle.Utilities.Helpers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
	.Host
	.AddConfig(configuration)
	.AddSerilog(configuration, AppDomain.CurrentDomain.FriendlyName)
	.ConnectOrleansClient(configuration);

builder.Services.AddHttpClient<IExternalProfileService, VeeFriendsProfileService>((client) =>
{
	client.BaseAddress = new Uri("https://veefriends.com/");
	client.Timeout = TimeSpan.FromMinutes(1);
});

builder.Services.ConfigureFastEndpoints(configuration);
builder.Services.AddWebAppApplicationInsights();
builder.Services.AddTransient<IClientConnectionRetryFilter, OrleansClientConnectionRetry>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.ConfigureFastEndpoints();

app.Run(); 