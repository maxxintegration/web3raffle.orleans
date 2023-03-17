namespace Web3raffle.Utilities.Helpers
{
	public class GuidBagHelper
	{
		private readonly ConcurrentQueue<Guid> queue;

		public GuidBagHelper(int instanceCount = 15)
		{
			this.queue = new ConcurrentQueue<Guid>(Enumerable.Range(0, instanceCount).Select(x => SeededGuid(x)));
		}

		public Guid GetNext()
		{
			if (this.Count == 0 || this.queue.IsEmpty)
			{
				throw new ObjectDisposedException(nameof(GuidBagHelper));
			}

			if (this.queue.TryDequeue(out var result))
			{
				this.queue.Enqueue(result);
				return result;
			}

			return this.GetNext();
		}

		public int Count => this.queue.Count;

		public bool Contains(Guid id) => this.queue.Contains(id);

		public void CopyTo(Guid[] array, int arrayIndex) => this.queue.CopyTo(array, arrayIndex);

		public Guid[] ToArray() => this.queue.ToArray();

		private static Guid SeededGuid(int seed)
		{
			var random = new Random(seed);

			return Guid.Parse(string.Format("{0:X4}{1:X4}-{2:X4}-{3:X4}-{4:X4}-{5:X4}{6:X4}{7:X4}",
				random.Next(0, 0xffff), random.Next(0, 0xffff),
				random.Next(0, 0xffff),
				random.Next(0, 0xffff) | 0x4000,
				random.Next(0, 0x3fff) | 0x8000,
				random.Next(0, 0xffff), random.Next(0, 0xffff), random.Next(0, 0xffff)));
		}
	}
}