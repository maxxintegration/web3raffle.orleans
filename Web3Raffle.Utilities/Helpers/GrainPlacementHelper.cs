using Orleans.Placement;
using Orleans.Runtime;
using Orleans.Runtime.Placement;

namespace Web3raffle.Utilities.Helpers
{
	public class GrainPlacementHelper : IPlacementDirector
	{
		private const string PlacementKey = "dashboard";
		private readonly IManagementGrain managementGrain;

		public GrainPlacementHelper(IGrainFactory grainFactory)
		{
			this.managementGrain = grainFactory
				.GetGrain<IManagementGrain>(0);
		}

		public async Task<SiloAddress> OnAddActivation(PlacementStrategy strategy, PlacementTarget target, IPlacementContext context)
		{
			var activeSilos = await this.managementGrain
				.GetDetailedHosts(onlyActive: true);

			var silos = activeSilos
				.Where(x => !x.RoleName.Contains(PlacementKey, StringComparison.OrdinalIgnoreCase))
				.Select(x => x.SiloAddress)
				.OrderBy(x => Guid.NewGuid())
				.First();

			return silos;
		}
	}

	[GenerateSerializer]
	public sealed class VFGrainPlacementStrategy : PlacementStrategy
	{
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class VFGrainPlacementAttribute : PlacementAttribute
	{
		public VFGrainPlacementAttribute() : base(new VFGrainPlacementStrategy())
		{
		}
	}
}