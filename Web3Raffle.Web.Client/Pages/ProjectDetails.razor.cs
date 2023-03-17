using Microsoft.AspNetCore.Components;
using Web3raffle.Models.Data;
using Web3raffle.Web.Client.Auth.Services;

namespace Web3raffle.Web.Client.Pages;

public partial class ProjectDetails : ComponentBase, IDisposable
{

	internal CancellationTokenSource cancellationToken = new();

	[Parameter]
	public string ProjectUrlSlug { get; set; }


	[Inject]
	private IApiService ApiService { get; set; } = default!;

	[Inject]
	protected NavigationManager Navigation { get; set; }

	protected bool IsLoading = true;


	protected Web3RaffleProjectModel Project = new Web3RaffleProjectModel();

	protected bool IsInitialLoad => string.IsNullOrEmpty(this.Project.Name) && this.IsLoading;


	protected override async Task OnInitializedAsync()
	{
		await this.GetProject();
	}

	protected async Task GetProject()
	{
		string condition = $"$filter=urlSlug = '{this.ProjectUrlSlug}'";

		this.IsLoading = true;
		var projects = await this.ApiService.GetProjectsAsync(condition, this.cancellationToken.Token);

		if (projects != null && projects.Count > 0)
			this.Project = projects[0];

		this.IsLoading = false;

	}


	public void Dispose()
	{
		this.cancellationToken.Cancel();
		this.cancellationToken.Dispose();
	}

}