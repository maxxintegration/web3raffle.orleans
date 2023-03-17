using Blazored.LocalStorage;
using System.Security.Claims;

namespace VeeFriends.Web.Web3Raffle.Client.Auth.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{

    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous;

	private string _token = string.Empty;

    public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        this._httpClient = httpClient;
		this._localStorage = localStorage;
		this._anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {

		var _token = this._localStorage.GetItemAsync<string>("token");

		if (string.IsNullOrEmpty(this._token))
            return this.SignOutNow();
		else
			this._localStorage.SetItemAsync("token", _token);

		return Task.FromResult(this.GetAuthenticationState());
    }

    public void SignIn()
    {
        var authState = Task.FromResult(this.GetAuthenticationState());
        this.NotifyAuthenticationStateChanged(authState);
    }

    public void SignOut()
    {
        var authState = this.SignOutNow();

       this.NotifyAuthenticationStateChanged(authState);
    }

    // CREATE USER CLAIMS PRINCIPAL
    private ClaimsPrincipal CreateClaimsPrincipal()
    {
		// DEFAULT USER NAME FOR NOW!!!
		string userName = "admin";

		// IMPLEMENT ROLES HERE!!!
		List<string> roles = new List<string> { };

        var claims = new Claim[roles.Count + 1];

        claims[0] = new Claim(ClaimTypes.Name, userName);
        for (int x = 0; x < roles.Count; x++)
        {
            claims[x + 1] = new Claim(ClaimTypes.Role, roles[x]);
        }

        var identity = new ClaimsIdentity(claims, "API");
        var user = new ClaimsPrincipal(identity);

        return user;
    }

    private Task<AuthenticationState> SignOutNow()
    {
        return Task.FromResult(this._anonymous);
    }

    private AuthenticationState GetAuthenticationState()
    {
        return new AuthenticationState(CreateClaimsPrincipal());
    }
}