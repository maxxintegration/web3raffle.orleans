using Microsoft.AspNetCore.Components;
using Web3raffle.Models.Data;
using Web3raffle.Models.Responses;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Shared.Modals;

public partial class ActivityLogModal : ComponentBase, IDisposable
{

	internal CancellationTokenSource cancellationToken = new();

	[Parameter]
	public Web3RaffleResponseModel Raffle { get; set; }


	[Inject]
	private IApiService ApiService { get; set; } = default!;

	protected List<Web3RaffleEventLogModel> EventLogs { get; set; } = new List<Web3RaffleEventLogModel>();

	protected bool ShowDialog = false;

	public async Task Open()
	{
		this.ShowDialog = true;

		string uri = $"$orderby=createAt desc&$top=50&$skip=0";

		var res = await this.ApiService.GetActivitiesByRaffleAsync(this.Raffle.Id, uri, this.cancellationToken.Token);

		this.EventLogs = res.Data;

		this.StateHasChanged();
	}

	public void Close()
	{
		this.ShowDialog = false;
		this.StateHasChanged();
	}

	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
	}

}