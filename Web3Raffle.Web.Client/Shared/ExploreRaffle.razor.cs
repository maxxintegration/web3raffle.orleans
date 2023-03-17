using Microsoft.AspNetCore.Components;
using Web3raffle.Models.Responses;
using Web3raffle.Web.Client.Auth.Extensions;
using Web3raffle.Web.Client.Shared;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Shared;

public partial class ExploreRaffle : ComponentBase, IDisposable
{

	internal CancellationTokenSource cancellationToken = new();

	[Parameter]
	public string? ProjectId { get; set; } = default;

	[Inject]
	private IApiService ApiService { get; set; } = default!;

	[Inject]
	protected NavigationManager Navigation { get; set; }

	protected List<Web3RaffleResponseModel> Raffles = new List<Web3RaffleResponseModel>();

	protected Pagination PaginationComponent;

	protected int DefaultPageSize = 20;
	protected int DefaultLength = 0;

	protected bool IsInitialLoad = true;
	protected bool IsLoading = true;

	int _status = -1;
	int _order = 1;

	protected override async Task OnInitializedAsync()
	{
		await this.GetRaffles();

		this.IsInitialLoad = false;
	}

	protected async Task GetRaffles()
	{
		string orderField = this._order == 1 ? "startDate" : "endDate",
			   condition = $"$orderby={orderField} desc";

		if (!string.IsNullOrEmpty(this.ProjectId))
			condition = $"{condition}&$filter=projectId = '{this.ProjectId}'";

		if (this._status > -1)
			condition = $"{condition}{(!string.IsNullOrEmpty(this.ProjectId) ? " and " : "&$filter=")}status = {this._status}";

		condition += $"&$top={(this.PaginationComponent == null ? this.DefaultPageSize : this.PaginationComponent.Top)}&$skip={(this.PaginationComponent == null ? 0 : this.PaginationComponent.Skip)}";

		var res = await this.ApiService.GetRafflesAsync(condition, this.cancellationToken.Token);
		this.Raffles = res.Data;

		if (this.PaginationComponent != null)
			this.PaginationComponent.SetLength(res.Length!.Value);
		else
			this.DefaultLength = res.Length!.Value;

	}

	protected string GotoRaffleEntrant(Web3RaffleResponseModel raffle)
	{
		string title = raffle.Name.ToUrlSlug(),
			   year = string.Format("{0:yyyy}", raffle.StartDate),
			   month = string.Format("{0:MM}", raffle.StartDate),
			   day = string.Format("{0:dd}", raffle.StartDate);

		return $"{this.Navigation.BaseUri}{raffle.ProjectName.ToUrlSlug()}/{year}/{month}/{day}/{title}";
	}

	protected string GotoProjectDetails(string projectName)
	{
		return $"{this.Navigation.BaseUri}{projectName.ToUrlSlug()}";
	}

	protected async Task OnStatusSelect(ChangeEventArgs e)
	{
		this._status = Convert.ToInt32(e.Value);
		await this.GetRaffles();
	}
	protected async Task OnOrderBySelect(ChangeEventArgs e)
	{
		this._order = Convert.ToInt32(e.Value);
		await this.GetRaffles();
	}

	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
	}

}