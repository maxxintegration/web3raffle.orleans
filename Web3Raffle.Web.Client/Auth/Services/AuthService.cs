using System.Net.Http.Json;
using VeeFriends.Web.Web3Raffle.Client.Auth.Consts;

namespace VeeFriends.Web.Web3Raffle.Client.Auth.Services;

public class AuthService : IAuthService
{

	private readonly HttpClient _client;

	public AuthService(IHttpClientFactory httpClient,
					   AuthenticationStateProvider authenticationStateProvider)
	{
		this._client = httpClient.CreateClient(HttpClientName.HttpClient);
	}

	public async Task<HttpResponseMessage> HttpClientAsync<T>(HttpMethod method, string url, T obj)
	{
		HttpResponseMessage response;

		var requestTimestamp = DateTimeOffset.UtcNow;

		response = await this.MakeRequest(method, url, obj);

		return response;
	}

	private async Task<HttpResponseMessage> MakeRequest<T>(HttpMethod method, string url, T obj)
	{

		return method.Method switch
		{
			"POST" => await this._client.PostAsJsonAsync(url, obj),
			"PUT" => await this._client.PutAsJsonAsync(url, obj),
			"DELETE" => await this._client.DeleteAsync(url),
			_ => await this._client.GetAsync(url)
		};

	}

}