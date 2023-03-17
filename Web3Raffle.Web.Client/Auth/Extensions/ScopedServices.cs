using Web3raffle.Web.Client.Auth.Services;
//using VeeFriends.Web.Web3Raffle.Client.Auth.Providers;

namespace Auth.Extensions
{
	public static class ScopedServices
	{
		public static IServiceCollection AddScopedServices(this IServiceCollection services)
		{
			//services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
			//services.AddScoped<IAuthService, AuthService>();
			//services.AddScoped<IDataService, DataService>();

			//services.AddScoped<IApiService, ApiService>();

			return services;
		}
	}
}