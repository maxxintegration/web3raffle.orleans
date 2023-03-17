using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Formats.Asn1;
using Web3raffle.Web.Client.Auth;
using Web3raffle.Models.Data;
using Web3raffle.Models.Responses;
using Web3raffle.Web.Client.Auth.Extensions;
using Web3raffle.Web.Client.Shared.Modals;
using Web3raffle.Web.Client.Shared;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Pages;

public partial class RaffleDetails : ComponentBase, IDisposable
{
	[Parameter]
	public string ProjectName { get; set; }

	[Parameter]
	public int Year { get; set; } = 0;

	[Parameter]
	public string Month { get; set; } = "00";

	[Parameter]
	public string Day { get; set; } = "00";

	[Parameter]
	public string RaffleTitle { get; set; } = string.Empty;

	[Inject]
	private IApiService ApiService { get; set; } = default!;

	[Inject]
	protected NavigationManager Navigation { get; set; }

	[Inject]
	protected IConfiguration Configuration { get; set; }

	[Inject]
	protected IJSRuntime JS { get; set; }

	private string _urlSlug = string.Empty;
	public string UrlSlug
	{
		get
		{
			if (string.IsNullOrEmpty(this._urlSlug))
				this._urlSlug = $"{this.RaffleTitle}_{this.Year}-{this.Month}-{this.Day}";

			return this._urlSlug;
		}
	}

	protected ActivityLogModal ActivityLogModalComponent;
	protected AddEntrantModal AddEntrantModalComponent;
	protected DidIWinModal DidIWinModalComponent;
	protected Pagination PaginationComponent;

	protected int DefaultPageSize = 50;

	protected string BaseApiUrl => this.Configuration.GetValue<string>("Web3RaffleApiUrl");

	internal CancellationTokenSource cancellationToken = new();

	protected string CurrentUrlEncode => System.Net.WebUtility.UrlEncode(this.Navigation.Uri);

	protected string RaffleTitleEncode => System.Net.WebUtility.UrlEncode(this.Raffle.Name);

	protected Web3RaffleResponseModel Raffle = new Web3RaffleResponseModel();


	protected List<Web3RaffleEntrantModel> Entrants = new List<Web3RaffleEntrantModel>();

	protected string SearchText { get; set; }

	protected bool IsInitialLoad => string.IsNullOrEmpty(this.Raffle.RaffleBannerUrl) && this.IsLoading;

	protected bool IsViewAllParticipant = true;
	protected bool IsLoading = true;

	protected string Error = string.Empty;

	string GetEntrantUri()
	{
		return $"$orderby=createAt desc&$top={this.PaginationComponent.PageSize}&$skip={this.PaginationComponent.Skip}";
	}

	protected override async Task OnInitializedAsync()
	{
		//this.raffle = await this.ApiService.GetRaffleAsync(this.RaffleNameSlug!, this.cancellationToken.Token);
		await this.LoadData();
	}

	protected async Task LoadData()
	{
		await this.GetRaffle();
		await this.GetEntrants();
	}

	protected void ResetErrorMessage()
	{
		this.Error = string.Empty;
	}

	protected string GotoProjectDetails(string projectName)
	{
		return $"{this.Navigation.BaseUri}{projectName.ToUrlSlug()}";
	}

	protected async Task DidIWin()
	{
		this.Error = string.Empty;

		if (string.IsNullOrEmpty(this.SearchText))
			this.Error = "Wallet address or display name is required!";
		else
		{
			await this.DidIWinModalComponent.Open(this.Raffle, this.SearchText);
			this.SearchText = string.Empty;
		}
	}

	protected void AddEntrant()
	{
		this.AddEntrantModalComponent.Open();
	}

	protected async Task GetRaffle()
	{
		string condition = $"$filter=urlSlug = '{this.UrlSlug}'";
		var res = await this.ApiService.GetRafflesAsync(condition, this.cancellationToken.Token);

		if (res.Data != null && res.Data.Count() > 0)
			this.Raffle = res.Data[0];
	}

	protected async Task LoadPage()
	{
		if (this.IsViewAllParticipant)
			await this.GetEntrants();
		else
			await this.GetWinners();
	}

	protected async Task GetEntrants()
	{
		this.IsLoading = true;

		var res = await this.ApiService.GetRaffleEntrantsAsync(this.Raffle.Id, this.GetEntrantUri(), this.cancellationToken.Token);

		this.Entrants = res.Data;
		this.PaginationComponent.SetLength(res.Length!.Value);
		this.IsLoading = false;
	}


	protected async Task GetWinners()
	{
		this.IsLoading = true;

		var res = await this.ApiService.GetRaffleWinnersAsync(this.Raffle.Id, this.GetEntrantUri(), this.cancellationToken.Token);

		this.Entrants = res.Data;
		this.PaginationComponent.SetLength(res.Length!.Value);
		this.IsLoading = false;
	}


	protected async Task DownloadCalendar()
	{
		string url = $"{this.BaseApiUrl}raffles/{this.Raffle.Id}/calendar/download",
			   fileName = $"{this.Raffle.Id}_date.ics";

		await this.JS.InvokeVoidAsync("web3raffle.triggerFileDownload", fileName, url);
	}

	protected async Task DownloadEntrants()
	{
		string url = $"{this.BaseApiUrl}raffles/{this.Raffle.Id}/entrants/download",
			   fileName = $"{this.Raffle.Id}_source.csv";

		await this.JS.InvokeVoidAsync("web3raffle.triggerFileDownload", fileName, url);
	}


	protected async Task DownloadWinners()
	{
		string url = $"{this.BaseApiUrl}raffles/{this.Raffle.Id}/winners/download",
			   fileName = $"{this.Raffle.Id}_source.csv";

		await this.JS.InvokeVoidAsync("web3raffle.triggerFileDownload", fileName, url);
	}

	protected async Task ViewActivities()
	{
		await this.ActivityLogModalComponent.Open();
	}

	protected async Task ViewToggle(bool viewAllParticipant)
	{

		if (this.IsLoading)
			return;

		this.IsViewAllParticipant = viewAllParticipant;
		this.PaginationComponent.PageNumber = 1;
		this.PaginationComponent.SetLength(0);
		if (this.IsViewAllParticipant)
			await this.GetEntrants();
		else
			await this.GetWinners();
	}




	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
	}
}