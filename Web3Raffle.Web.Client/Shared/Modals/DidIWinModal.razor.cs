using Microsoft.AspNetCore.Components;
using Web3raffle.Models.Data;
using Web3raffle.Models.Responses;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Shared.Modals;

public partial class DidIWinModal : ComponentBase, IDisposable
{
	internal CancellationTokenSource cancellationToken = new();

	[Inject]
	private IApiService ApiService { get; set; } = default!;


	protected Web3RaffleResponseModel Raffle = new Web3RaffleResponseModel();
	protected List<Web3RaffleEntrantModel> Entrants = new List<Web3RaffleEntrantModel>();

	protected bool ShowDialog = false;
	protected bool IsSearching = false;


	string _searchText = string.Empty;

	public async Task Open(Web3RaffleResponseModel raffle, string searchText)
	{
		this.Entrants = new List<Web3RaffleEntrantModel>();
		this.Raffle = raffle;
		this._searchText = searchText;
		this.IsSearching = true;
		this.ShowDialog = true;
		this.StateHasChanged();


		await Task.Delay(2000);

		await this.GetWinners();

	}

	public void Close()
	{
		this.ShowDialog = false;
		this.StateHasChanged();
	}



	protected async Task GetWinners()
	{
		string condition = $"$filter=(walletAddress = '{this._searchText}' or displayName = '{this._searchText}')&$orderby=entrantSequence asc";
		var res = await this.ApiService.GetRaffleWinnersAsync(this.Raffle.Id, condition, this.cancellationToken.Token);

		this.Entrants = res.Data;

		this.IsSearching = false;

		this.StateHasChanged();
	}

	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
	}


}