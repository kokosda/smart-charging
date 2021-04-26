using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.Connectors
{
	public sealed record UpdateConnectorMaxCurrentSpecification : ISpecification<Connector, int>
	{
		private readonly decimal current;
		private readonly IGroupRepository groupRepository;

		public UpdateConnectorMaxCurrentSpecification(decimal current, IGroupRepository groupRepository)
		{
			this.current = current;
			this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
		}

		public async Task<IResponseContainer> IsSatisfiedBy(Connector connector)
		{
			if (connector is null)
				throw new ArgumentNullException(nameof(connector));

			var result = new ResponseContainer();

			if (connector.MaxCurrentInAmps == current)
			{
				result.AddErrorMessage($"Connector's [{connector.GetNumericId()}] current is equal to provided one ({current}).");
				return result;
			}

			if (current <= 0)
				result.AddErrorMessage($"Connector's [{connector.GetNumericId()}] current can not be a value less than or equal to 0. Value provided: {current}.");
			else
			{
				var group = await groupRepository.GetByConnectorAsync(connector);
				var responseContainer = await new UpdateGroupCurrentSpecification(current, connector.MaxCurrentInAmps).IsSatisfiedBy(group);

				result.JoinWith(responseContainer);
			}

			return result;
		}
	}
}
