using System;
using System.Threading.Tasks;
using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Connectors
{
	public sealed class UpdateMaxCurrentConnectorHandler : CommandHandlerBase<UpdateConnectorRequest>, IUpdateMaxCurrentConnectorHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(UpdateMaxCurrentConnectorHandler));
		private readonly IConnectorRepository connectorRepository;
		private readonly IGroupRepository groupRepository;

		public UpdateMaxCurrentConnectorHandler(IConnectorRepository connectorRepository, IGroupRepository groupRepository)
		{
			this.connectorRepository = connectorRepository ?? throw new ArgumentNullException(nameof(connectorRepository));
			this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
		}

		protected override async Task<IResponseContainer> GetResultAsync(UpdateConnectorRequest request)
		{
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
