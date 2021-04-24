using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Connectors
{
	public sealed class UpdateConnectorHandler : IUpdateConnectorHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(UpdateConnectorHandler));
		private readonly IConnectorRepository connectorRepository;
		private readonly IGroupRepository groupRepository;

		public UpdateConnectorHandler(IConnectorRepository connectorRepository, IGroupRepository groupRepository)
		{
			this.connectorRepository = connectorRepository ?? throw new ArgumentNullException(nameof(connectorRepository));
			this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
		}

		public async Task<IResponseContainer> UpdateMaxCurrentAsync(UpdateConnectorRequest request)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			IResponseContainer result = new ResponseContainer();
			var connector = await connectorRepository.GetByChargeStationIdAndLineNo(request.ChargeStationId, request.LineNo);

			if (connector is null)
			{
				result.AddErrorMessage($"Connector with ID={request.ChargeStationId} is not found.");
				return result;
			}

			(await connector.UpdateMaxCurrrentInAmps(request.MaxCurrentInAmps, groupRepository)).JoinWith(ref result);

			if (result.IsSuccess)
			{
				var previousValue = connector.MaxCurrentInAmps;
				await connectorRepository.UpdateAsync(connector);

				Log.LogInfo($"Connector with ID={connector.GetNumericId()} is updated with new value {request.MaxCurrentInAmps}, previous value was {previousValue}.");
			}
			else
				Log.LogError(result.Messages);

			return result;
		}
	}
}
