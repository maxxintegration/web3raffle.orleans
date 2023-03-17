using System.Net.Http.Json;
using Web3raffle.Models.Data;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Web.Client.Auth.Extensions;

namespace Web3raffle.Web.Client.Auth.Services;

public class ApiService : IApiService
{
	private readonly HttpClient http;

	public ApiService(HttpClient http)
	{
		this.http = http;
	}

	public async Task<ResponseCollectionModel<TModel>> Gets<TModel>(string requestUri, CancellationToken ct) where TModel : BaseResponseDataModel, new()
	{

		var response = await this.http.GetAsync(requestUri);

		response.EnsureSuccessStatusCode();

		string result = response.Content.ReadAsStringAsync(ct).Result;

		ArgumentNullException.ThrowIfNull(result);

		var data = result.JsonToObject<ResponseCollectionModel<TModel>>();

		ArgumentNullException.ThrowIfNull(data.Data);

		return data;

	}

	public async Task<ResponseModel<TModel>> Get<TModel>(string requestUri, CancellationToken ct) where TModel : BaseResponseDataModel, new()
	{

		var response = await this.http.GetAsync(requestUri);

		response.EnsureSuccessStatusCode();

		string result = response.Content.ReadAsStringAsync(ct).Result;

		ArgumentNullException.ThrowIfNull(result);

		var data = result.JsonToObject<ResponseModel<TModel>>();

		ArgumentNullException.ThrowIfNull(data.Data);

		return data;

	}

	public async Task<List<Web3RaffleProjectModel>> GetProjectsAsync(string condition, CancellationToken ct)
	{
		var data = await this.Gets<Web3RaffleProjectModel>($"projects?{condition}", ct);

		return data.Data;
	}

	public async Task<Web3RaffleResponseModel> GetRaffleAsync(string raffleId, CancellationToken ct)
	{
		var data = await this.Get<Web3RaffleResponseModel>($"raffles/{raffleId}", ct);

		return data.Data;
	}

	public async Task<ResponseCollectionModel<Web3RaffleResponseModel>> GetRafflesAsync(string condition, CancellationToken ct)
	{
		var data = await this.Gets<Web3RaffleResponseModel>($"raffles?{condition}", ct);

		return data;
	}

	public async Task<ResponseCollectionModel<Web3RaffleEntrantModel>> GetRaffleEntrantsAsync(string raffleId, string condition, CancellationToken ct)
	{
		var data = await this.Gets<Web3RaffleEntrantModel>($"raffles/{raffleId}/entrants?{condition}", ct);

		return data;
	}

	public async Task<ResponseCollectionModel<Web3RaffleEntrantModel>> GetRaffleWinnersAsync(string raffleId, string condition, CancellationToken ct)
	{
		var data = await this.Gets<Web3RaffleEntrantModel>($"raffles/{raffleId}/winners?{condition}", ct);

		return data;
	}

	public async Task<List<Web3RaffleEntrantModel>> CreateEntrantAsync(Web3RaffleEntrantModel entrant, CancellationToken ct)
	{

		var param = new Web3RaffleEntrantRequestModel();
		param.Data = new List<Web3RaffleEntrantModel>() { entrant };

		var response = await this.http.PostAsJsonAsync($"raffles/{entrant.RaffleId}/entrants", param, ct);

		string result = response.Content.ReadAsStringAsync(ct).Result;

		var data = result.JsonToObject<ResponseCollectionModel<Web3RaffleEntrantModel>>();

		return data.Data;

	}


	public async Task<ResponseCollectionModel<Web3RaffleEventLogModel>> GetActivitiesByRaffleAsync(string raffleId, string condition, CancellationToken ct)
	{
		var data = await this.Gets<Web3RaffleEventLogModel>($"activities/{raffleId}?{condition}", ct);

		return data;
	}


}