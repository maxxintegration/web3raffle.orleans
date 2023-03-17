namespace Web3raffle.Abstractions.GrainInterfaces
{
	public interface IAutomicNumberGrain : IGrainWithGuidKey
	{
		Task<int> GetCurrent();

		Task<int> GetNext();

		Task Reset();

		Task ChangeState(int state);
	}
}