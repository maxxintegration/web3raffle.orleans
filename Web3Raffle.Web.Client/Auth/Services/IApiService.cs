using Web3raffle.Models.Data;
using Web3raffle.Models.Responses;

namespace Web3raffle.Web.Client.Auth.Services;

public interface IApiService
{
	Task<ResponseModel<TModel>> Get<TModel>(string requestUri, CancellationToken ct) where TModel : BaseResponseDataModel, new();
	Task<ResponseCollectionModel<TModel>> Gets<TModel>(string requestUri, CancellationToken ct) where TModel : BaseResponseDataModel, new();
	Task<List<Web3RaffleProjectModel>> GetProjectsAsync(string condition, CancellationToken ct);
	Task<Web3RaffleResponseModel> GetRaffleAsync(string raffleId, CancellationToken ct);
	Task<ResponseCollectionModel<Web3RaffleResponseModel>> GetRafflesAsync(string condition, CancellationToken ct);
	Task<ResponseCollectionModel<Web3RaffleEntrantModel>> GetRaffleEntrantsAsync(string raffleId, string condition, CancellationToken ct);
	Task<ResponseCollectionModel<Web3RaffleEntrantModel>> GetRaffleWinnersAsync(string raffleId, string condition, CancellationToken ct);
	Task<List<Web3RaffleEntrantModel>> CreateEntrantAsync(Web3RaffleEntrantModel entrant, CancellationToken ct);
	Task<ResponseCollectionModel<Web3RaffleEventLogModel>> GetActivitiesByRaffleAsync(string raffleId, string condition, CancellationToken ct);
}