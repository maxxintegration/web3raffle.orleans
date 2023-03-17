namespace Web3raffle.Web.Client.Store.App;


//https://github.com/mrpmorris/Fluxor/blob/master/Source/Tutorials/02-Blazor/02E-ActionSubscriber/FluxorBlazorWeb.ActionSubscriberTutorial/Client/Store/CustomerUseCases/EditUseCases/EditCustomerEffects.cs
public record AppState
{
	public string UserId { get; init; } = string.Empty;
	public string UserName { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
}

public class AppFeatureState : Feature<AppState>
{
	public override string GetName() => nameof(AppState);

	protected override AppState GetInitialState()
	{
		return new AppState
		{
			UserId = string.Empty,
			UserName = string.Empty,
			Email = string.Empty
		};
	}
}