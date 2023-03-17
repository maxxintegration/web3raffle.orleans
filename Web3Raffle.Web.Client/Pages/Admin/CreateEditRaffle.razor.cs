using Microsoft.AspNetCore.Components;

namespace Web3raffle.Web.Client.Pages.Admin;

public partial class CreateEditRaffle : ComponentBase
{

	[Parameter]
	public string Mode { get; set; }

	[Parameter]
	public string Id { get; set; }

}