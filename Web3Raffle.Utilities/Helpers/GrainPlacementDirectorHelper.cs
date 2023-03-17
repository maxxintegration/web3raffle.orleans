using Orleans;
using Orleans.Placement;
using Orleans.Runtime;
using Orleans.Runtime.Placement;

namespace Web3raffle.Utilities.Helpers
{
	public class GrainPlacementDirectorHelper : IPlacementDirector
	{
		public IGrainFactory grainFactory { get; set; }
		public IManagementGrain managementGrain { get; set; }

		public GrainPlacementDirectorHelper(IGrainFactory grainFactory)
		{
			this.grainFactory = grainFactory;
			this.managementGrain = this.grainFactory.GetGrain<IManagementGrain>(0);
		}

		public async Task<SiloAddress> OnAddActivation(PlacementStrategy strategy, PlacementTarget target, IPlacementContext context)
		{
			var activeSilos = await this.managementGrain.GetDetailedHosts(onlyActive: true);
			var silos = activeSilos.Where(x => !x.RoleName.ToLower().Contains("dashboard")).Select(x => x.SiloAddress).ToArray();
			return silos[new Random().Next(0, silos.Length)];
		}
	}

	[GenerateSerializer]
	public sealed class DontPlaceMeOnTheDashboardStrategy : PlacementStrategy
	{
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class DontPlaceMeOnTheDashboardAttribute : PlacementAttribute
	{
		public DontPlaceMeOnTheDashboardAttribute() :
			base(new DontPlaceMeOnTheDashboardStrategy())
		{
		}
	}
}