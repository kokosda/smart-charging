using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors.Factories;
using SmartCharging.Domain.Groups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors.Strategies
{
	public sealed class AllocationThroughMinNumberExclusionStrategy : IConnectorAllocationStrategy
	{
		private readonly IConnectorRepository connectorRepository;
		private readonly IGroupRepository groupRepository;

		public AllocationThroughMinNumberExclusionStrategy(IConnectorRepository connectorRepository, IGroupRepository groupRepository)
		{
			this.connectorRepository = connectorRepository;
			this.groupRepository = groupRepository;
		}

		public async Task<IResponseContainerWithValue<IReadOnlyList<CreateConnectorSuggestion>>> AllocateAsync(int chargeStationId, decimal maxCurrentInAmps)
		{
			IResponseContainerWithValue<IReadOnlyList<CreateConnectorSuggestion>> result;

			var group = await groupRepository.GetByChargeStationIdAsync(chargeStationId);
			var occupiedCapacity = group.GetOccupiedCapacity();
			var suggestions = new List<CreateConnectorSuggestion>();

			if (group.CapacityInAmps < maxCurrentInAmps)
			{
				result = new ResponseContainerWithValue<IReadOnlyList<CreateConnectorSuggestion>>();
				result.AddErrorMessage($"Max current {maxCurrentInAmps} provided exceeds group's {group.Name} capacity.");
				return result;
			}

			if (!group.WillBecomeOvercapped(occupiedCapacity, maxCurrentInAmps))
				return new ResponseContainerWithValue<IReadOnlyList<CreateConnectorSuggestion>> { Value = suggestions };

			var connectors = await connectorRepository.GetAllInGroupByChargeStationIdAsync(chargeStationId);

			throw new NotImplementedException();
		}
	}
}
