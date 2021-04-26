using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed class ConnectorFactory : IConnectorFactory
	{
		private readonly IConnectorRepository connectorRepository;
		private readonly IGenericRepository<ChargeStation, int> chargeStationRepository;
		private readonly IGroupRepository groupRepository;

		public ConnectorFactory(
			IConnectorRepository connectorRepository, 
			IGenericRepository<ChargeStation, int> chargeStationRepository, 
			IGroupRepository groupRepository)
		{
			this.connectorRepository = connectorRepository;
			this.chargeStationRepository = chargeStationRepository;
			this.groupRepository = groupRepository;
		}

		public async Task<IResponseContainerWithValue<ConnectorFactoryResult>> CreateAsync(int chargeStationId, int lineNo, decimal maxCurrentInAmps)
		{
			var result = new ResponseContainerWithValue<ConnectorFactoryResult>();
			var existingConnector = await connectorRepository.GetByChargeStationIdAndLineNoAsync(chargeStationId, lineNo);

			if (existingConnector is not null)
			{
				result.AddErrorMessage($"There is already a connector exising with charge station ID={chargeStationId} and line #{lineNo}.");
				return result;
			}

			var connectors = connectorRepository.GetAllInGroupByChargeStationIdAsync(chargeStationId);

			return result;
		}
	}
}
