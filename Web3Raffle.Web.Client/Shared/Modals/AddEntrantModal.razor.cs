using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Web3raffle.Models.Data;
using Web3raffle.Models.Responses;
using Web3raffle.Shared;
using Web3raffle.Web.Client.Auth.Extensions;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Shared.Modals;

public partial class AddEntrantModal : ComponentBase, IDisposable
{

	internal CancellationTokenSource cancellationToken = new();

	[Parameter]
	public Web3RaffleResponseModel Raffle { get; set; }


	[Parameter]
	public EventCallback<string> OnEntrantCreate { get; set; }

	[Inject]
	private IApiService ApiService { get; set; } = default!;

	[Inject]
	public HttpInterceptorService Interceptor { get; set; }


	protected Web3RaffleEntrantModel Entrant { get; set; } = new Web3RaffleEntrantModel();


	protected EditForm EntrantForm { get; set; }


	protected bool ShowDialog = false;
	protected bool IsUpdate = false;
	protected List<string> Errors;



	protected override Task OnInitializedAsync()
	{
		this.Interceptor.RegisterEvent();
		return Task.CompletedTask;
	}

	public void Open()
	{
		this.Entrant = new Web3RaffleEntrantModel();
		this.Errors = new List<string>();
		this.Entrant.RaffleId = this.Raffle.Id;
		this.IsUpdate = false;
		this.ShowDialog = true;
		this.Interceptor.Response = null;
		this.StateHasChanged();
	}

	public void Close()
	{
		if (this.IsUpdate)
			return;

		this.ShowDialog = false;
		this.StateHasChanged();
	}

	public async Task Create()
	{
		if (this.IsUpdate)
			return;

		this.Errors = new List<string>();
		this.IsUpdate = true;
		this.Interceptor.Response = null;

		if (string.IsNullOrEmpty(this.Entrant.WalletAddress))
			this.Errors.Add("Wallet Address is required!");
		else if (this.Raffle.RequiredEmail && string.IsNullOrEmpty(this.Entrant.Email))
			this.Errors.Add("Email Address is required!");
		else if (this.Raffle.RequiredPhoneNumber && string.IsNullOrEmpty(this.Entrant.Phone))
			this.Errors.Add("Phone is required!");
		else
		{
			await this.ApiService.CreateEntrantAsync(this.Entrant, this.cancellationToken.Token);

			// CHECK FOR ERROR
			var error = await this.GetError();
			if (error != null && error!.Messages!.Count > 0)
			{
				this.Errors = error.Messages;
			}
			else
			{
				if (this.OnEntrantCreate.HasDelegate)
					await this.OnEntrantCreate.InvokeAsync("");

				this.IsUpdate = false;
				this.Close();
			}

		}

		this.IsUpdate = false;
	}

	private async Task<ErrorModel> GetError()
	{
		ErrorModel error = default!;

		if (this.Interceptor.Response != null && !this.Interceptor.Response!.IsSuccessStatusCode && this.Interceptor.Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
		{
			var content = await this.Interceptor.Response.Content.ReadAsStringAsync();
			error = content.JsonToObject<ErrorModel>();
		}

		return error;
	}

	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
		this.Interceptor.DisposeEvent();
	}

}