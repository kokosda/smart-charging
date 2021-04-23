using System;
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

		public IResponseContainer IsSatisfiedBy(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new ResponseContainer();
			var connectorSpecification = new UpdateConnectorMaxCurrentSpecification { Current = current };
			var connectorSpecificationResponse = connectorSpecification.IsSatisfiedBy(connector);
			result.JoinWith(connectorSpecificationResponse);

			if (result.IsSuccess)
			{
				var isOvercapped = group.CapacityInAmps < (group.CapacityInAmps - connector.MaxCurrentInAmps + current);

				if (isOvercapped)
					result.AddErrorMessage($"Updating connector's [{connector.GetNumericId()}] current will overflow group's [{group.Name}] current capacity.");
			}

			return result;
		}
	}
}
