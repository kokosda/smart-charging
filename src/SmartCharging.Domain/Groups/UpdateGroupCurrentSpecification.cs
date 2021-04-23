using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Domain.Groups
{
	public sealed record UpdateGroupCurrentSpecification : ISpecification<Group, int>
	{
		private readonly decimal current;
		private readonly Connector connector;

		public UpdateGroupCurrentSpecification(decimal current, Connector connector)
		{
			this.current = current;
			this.connector = connector ?? throw new ArgumentNullException(nameof(connector));
		}

		public Task<IResponseContainer> IsSatisfiedBy(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new ResponseContainer();
			var isOvercapped = group.CapacityInAmps < (group.CapacityInAmps - connector.MaxCurrentInAmps + current);

			if (isOvercapped)
				result.AddErrorMessage($"Updating connector's [{connector.GetNumericId()}] current will overflow group's [{group.Name}] current capacity {group.CapacityInAmps}.");

			return Task.FromResult(result.AsInterface());
		}
	}
}
