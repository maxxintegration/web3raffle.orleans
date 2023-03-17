using Web3raffle.Models.Data;
using Web3raffle.Utilities.Extensions;

namespace Web3raffle.Silo.Extensions
{
	public static class SiloServiceCollectionExtensions
	{
		public static IServiceCollection AddCosmosClientWeb3Raffle(this IServiceCollection services)
		{
			services.AddCosmosClient<Web3RaffleProjectModel>();
			services.AddCosmosClient<Web3RaffleEventLogModel>();
			services.AddCosmosClient<Web3RaffleModel>();
			services.AddCosmosClient<Web3RaffleBlacklistModel>();
			services.AddCosmosClient<Web3RaffleWhitelistModel>();
			services.AddCosmosClient<Web3RaffleEntrantModel>();

			return services;
		}
	}
}