namespace VeeFriends.Web.Web3Raffle.Client.Auth.Services;

public interface IAuthService
{
    Task<HttpResponseMessage> HttpClientAsync<T>(HttpMethod method, string url, T obj);
}