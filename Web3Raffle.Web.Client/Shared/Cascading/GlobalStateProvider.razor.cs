using Microsoft.AspNetCore.Components;
using VeeFriends.Models.Data.Web3Raffle;
using VeeFriends.Models.Enums;
using VeeFriends.Web.Web3Raffle.Client.Auth.Services;

namespace VeeFriends.Web.Web3Raffle.Client.Shared.Cascading;

public partial class GlobalStateProvider : ComponentBase
{
	[Inject]
	public GlobalState globalState { get; set; }

	[Inject]
	public IDataService DataService { get; set; }

	[Inject]
	public IAuthService AuthService { get; set; }

	[Parameter]
	public RenderFragment ChildContent { get; set; }

	[CascadingParameter]
	public Task<AuthenticationState> AuthenticationStateTask { get; set; }

	public GlobalState App { get; set; }

	private async Task LogActivity(Web3RaffleEventLogModel model)
	{
		ResponseDataModel res = await this.DataService.LogActivity(model);
	}

	public async Task<System.Security.Claims.ClaimsPrincipal> UserAsync()
	{
		System.Security.Claims.ClaimsPrincipal user = null!;

		if (this.AuthenticationStateTask != null)
		{
			user = (await this.AuthenticationStateTask).User;
		}

		return user!;
	}

	protected override async Task OnInitializedAsync()
	{
		this.App = this.globalState;
		this.App.DataService = this.DataService;

		this.App.OnChange += this.StateHasChanged;

		var token = await this.App.GetToken();

		if (token != null)
		{
			try
			{
				await this.AuthService.Login(token);
			}
			catch (Exception e)
			{
				var m = e.Message;
				var n = m;
			}
		}

		this.StateHasChanged();
	}

	public async Task<Web3RaffleModel> GetRaffleInfo(string raffleId, Func<Web3RaffleModel, Task> callback = null!)
	{
		Web3RaffleModel raffle = new Web3RaffleModel();
		//var res = await DataService.GetRaffle(raffleId);
		//if (res.StatusCode == System.Net.HttpStatusCode.OK)
		//{
		//    raffle = ((ItemResponseModel<Web3RaffleModel>)res.Data).Data;
		//    if(callback != null)
		//    {
		//        await callback(raffle);
		//    }
		//}

		return raffle;
	}

	public async Task LogActivity(string raffleId, EventLogType eventLog, string logText, bool getRaffleInfo = false)
	{
		Web3RaffleEventLogModel model = new Web3RaffleEventLogModel
		{
			RaffleId = raffleId,
			LogType = eventLog,
			LogMessage = logText
		};

		if (getRaffleInfo)
		{
			await this.GetRaffleInfo(raffleId, async (raffleView) =>
			{
				model.LogMessage = string.Format(model.LogMessage, raffleView.Name);
				await this.LogActivity(model);
			});
		}
		else
		{
			await this.LogActivity(model);
		}
	}
}