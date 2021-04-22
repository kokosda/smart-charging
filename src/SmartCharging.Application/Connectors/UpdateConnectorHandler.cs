using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Connectors
{
	public sealed class UpdateConnectorHandler : IUpdateConnectorHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(UpdateConnectorHandler));
		private readonly IConnectorRepository connectorRepository;

		public UpdateConnectorHandler(IConnectorRepository connectorRepository)
		{
			this.connectorRepository = connectorRepository ?? throw new ArgumentNullException(nameof(connectorRepository));
		}

		public async Task UpdateMaxCurrentAsync(UpdateConnectorRequest request)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var connector = await connectorRepository.GetByChargeStationIdAndLineNo(request.ChargeStationId, request.LineNo);

			if (connector is null)
				throw new NullReferenceException($"Connector with ID={request.ChargeStationId} is not found.");

			var previousValue = connector.MaxCurrentInAmps;
			connector.UpdateMaxCurrrentInAmps(request.MaxCurrentInAmps);

			await connectorRepository.UpdateAsync(connector);

			Log.LogInfo($"Connector with ID={connector.GetNumericId()} is updated with new value {request.MaxCurrentInAmps}, previous value was {previousValue}.");
		}
	}
}
