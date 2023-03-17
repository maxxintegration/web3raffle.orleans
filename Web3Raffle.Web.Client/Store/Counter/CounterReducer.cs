using Fluxor;

namespace Web3raffle.Web.Client.Store.Counter;

public static class CounterReducer
{
	[ReducerMethod]
	public static CounterState OnAddCounter(CounterState state, AddCounter action)
	{


		return state with
		{
			Count = state.Count + 1
		};

	}
}