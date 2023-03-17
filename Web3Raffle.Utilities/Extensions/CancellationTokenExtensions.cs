namespace Web3raffle.Utilities.Extensions
{
	public static class CancellationTokenExtensions
	{
		public static GrainCancellationToken ToGrainCancellationToken(this CancellationToken cancellationToken)
		{
			var grainCancellationToken = new GrainCancellationTokenSource();
			var token = grainCancellationToken.Token;

			var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

			token.CancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false);

			return token;
		}
	}
}