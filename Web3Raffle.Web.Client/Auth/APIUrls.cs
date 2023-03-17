namespace Web3raffle.Web.Client.Auth;

public class APIUrls
{
	public const string TokenUrl = "token";
	public const string TokenRefreshUrl = "token/refresh";
	public const string AccountGetUserInfoUrl = "account/user-info";

	public const string RaffleUrl = "raffles";
	public const string RaffleCurrentUrl = "raffles/current";
	public const string RaffleCreateUrl = "raffles/create";
	public const string RaffleUpdateUrl = "raffles/edit";
	public const string RaffleDeleteUrl = "raffles/delete";
	public const string RaffleDrawUrl = "raffles/draw";
	public const string RaffleEntrantsUrl = "raffles/{raffleid}/entrants";
	public const string RaffleWinnersUrl = "raffles/{raffleid}/winners";
	public const string RaffleWinnerSearchUrl = "raffles/{raffleid}/winners/search";
	public const string RaffleEntrantSearchUrl = "raffles/{raffleid}/entrants/search";
	public const string RaffleActivitiesUrl = "activities/raffles/{raffleid}";

	public const string EntrantsUrl = "raffles/{raffleid}/entrants";
	public const string ActivitiesUrl = "activities";

	public const string ListsClearAllUrl = "lists/clear-all";
	public const string ListsClearWhiteListUrl = "raffles/clear-whitelist";
	public const string ListsClearBlackListUrl = "raffles/clear-blacklist";
	public const string ListsAddBlackListUrl = "raffles/add-blacklist";
	public const string ListsAddWhiteListUrl = "raffles/add-whitelist";
}