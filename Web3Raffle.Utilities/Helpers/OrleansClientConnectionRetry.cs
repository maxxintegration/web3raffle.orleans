using Orleans.Runtime;

namespace Web3raffle.Utilities.Helpers
{
	public class OrleansClientConnectionRetry : IClientConnectionRetryFilter
	{
		public async Task<bool> ShouldRetryConnectionAttempt(Exception exception, CancellationToken cancellationToken)
		{
			if (exception is SiloUnavailableException)
			{
				await Task.Delay(TimeSpan.FromSeconds(2));

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}