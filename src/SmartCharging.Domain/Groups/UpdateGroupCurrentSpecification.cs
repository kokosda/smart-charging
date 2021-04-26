using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;

namespace SmartCharging.Domain.Groups
{
	public sealed record UpdateGroupCurrentSpecification : ISpecification<Group, int>
	{
		private readonly decimal newMaxCurrentInAmps;
		private readonly decimal presentedMaxCurrentInAmps;

		public UpdateGroupCurrentSpecification(decimal newMaxCurrentInAmps, decimal currentMaxCurrentInAmps)
		{
			this.newMaxCurrentInAmps = newMaxCurrentInAmps;
			this.presentedMaxCurrentInAmps = currentMaxCurrentInAmps;
		}

		public Task<IResponseContainer> IsSatisfiedBy(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new ResponseContainer();
			var occupiedCapacity = group.GetOccupiedCapacity();
			var isOvercapped = group.WillBecomeOvercapped(occupiedCapacity, presentedMaxCurrentInAmps, newMaxCurrentInAmps);

			if (isOvercapped)
				result.AddErrorMessage($"Updating connector's [{presentedMaxCurrentInAmps}] current will overflow group's [{group.Name}] current capacity {group.CapacityInAmps}.");

			return Task.FromResult(result.AsInterface());
		}
	}
}
