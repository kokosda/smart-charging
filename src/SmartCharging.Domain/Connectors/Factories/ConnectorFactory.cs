using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors.Strategies;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed class ConnectorFactory : IConnectorFactory
	{
		private readonly IConnectorRepository connectorRepository;
		private readonly IConnectorAllocationStrategy connectorAllocationStrategy;

		public ConnectorFactory(
			IConnectorRepository connectorRepository,
			IConnectorAllocationStrategy connectorAllocationStrategy
		)
		{
			this.connectorRepository = connectorRepository;
			this.connectorAllocationStrategy = connectorAllocationStrategy;
		}

		public async Task<IResponseContainerWithValue<ConnectorFactoryResult>> CreateAsync(int chargeStationId, int lineNo, decimal maxCurrentInAmps)
		{
			var result = new ResponseContainerWithValue<ConnectorFactoryResult>();
			var existingConnector = await connectorRepository.GetByChargeStationIdAndLineNoAsync(chargeStationId, lineNo);

			if (existingConnector is not null)
			{
				result.AddErrorMessage($"There is already a connector existing with charge station ID={chargeStationId} and line #{lineNo}.");
				return result;
			}

			var strategyResponseContainer = await connectorAllocationStrategy.AllocateAsync(chargeStationId, maxCurrentInAmps);

			if (!strategyResponseContainer.IsSuccess)
				return result.JoinWith(strategyResponseContainer);

			Connector connector = null;

			if (!strategyResponseContainer.Value.Any())
			{
				connector = new Connector
				{
					ChargeStationId = chargeStationId,
					LineNo = lineNo,
					MaxCurrentInAmps = maxCurrentInAmps
				};

				result.AddMessage($"Suggestions are not made. Creating a connector. [{connector}]");
				connector = await connectorRepository.CreateAsync(connector);
				result.AddMessage($"Connector created. [{connector}]");
			}

			result = new ResponseContainerWithValue<ConnectorFactoryResult>
			{
				Value = new ConnectorFactoryResult(strategyResponseContainer.Value) {Connector = connector}
			};

			return result;
		}
	}
}
