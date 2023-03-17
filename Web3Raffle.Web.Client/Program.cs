using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Web3raffle.Web.Client;
using Web3raffle.Shared;
using Auth.Extensions;
using Web3raffle.Web.Client.Auth.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//builder.Services.AddFluxor(configs => {

//		configs.ScanAssemblies(typeof(Program).Assembly);
//		//#if DEBUG
//			configs.UseReduxDevTools();
//		//#endif
//});

builder.Services.AddFluxor(o => o
	.ScanAssemblies(typeof(Program).Assembly)
	.UseRouting()
	.UseReduxDevTools());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<IApiService, ApiService>("Web3RaffleAPI", (provider, client) =>
{
	client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Web3RaffleApiUrl")!);
	client.DefaultRequestHeaders.Add("Accept", "application/json");
	client.EnableIntercept(provider);
});

builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();

builder.Services.AddScopedServices();

await builder.Build().RunAsync();

























/* TEMPORARY COMMENT THIS SERVER CODE
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using VeeFriends.Web.Web3Raffle.Client.Auth.Providers;
//using VeeFriends.Web.Web3Raffle.Client.Auth.Services;
//using VeeFriends.Web.Web3Raffle.Client.Auth.Services.IDataServices;
using VeeFriends.Web.Web3Raffle.Client;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddFluxor(configs => {

//		configs.ScanAssemblies(typeof(Program).Assembly);
//		//#if DEBUG
//			configs.UseReduxDevTools();
//		//#endif
//});



builder.Services.AddFluxor(o => o
	.ScanAssemblies(typeof(Program).Assembly)
	.UseRouting()
	.UseReduxDevTools());




//builder.Services.AddScoped<IStore, Store>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


//builder.Services.AddScoped<GlobalState>();
//builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IDataService, VeeFriends.Web.Web3Raffle.Client.Auth.Services.DataServices.DataService>();

await builder.Build().RunAsync().ConfigureAwait(false);

*/
