using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Logging;
using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public sealed class DeleteConnectorHandler : CommandHandlerBase<DeleteConnectorRequest>, IDeleteConnectorHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(DeleteConnectorHandler));
		private readonly IConnectorRepository connectorRepository;

		public DeleteConnectorHandler(IConnectorRepository connectorRepository)
		{
			this.connectorRepository = connectorRepository;
		}

		protected override async Task<IResponseContainer> GetResultAsync(DeleteConnectorRequest request)
		{
			var result = new ResponseContainer();
			var connector = await connectorRepository.GetByChargeStationIdAndLineNoAsync(request.ChargeStationId, request.LineNo);

			if (connector is not null)
			{
				await connectorRepository.DeleteAsync(connector.Id);
				Log.Info($"Connector {connector} has been deleted.");
			}
			else
			{
				var message = $"Connector with charge station ID={request.ChargeStationId} and line #{request.LineNo} is not found.";
				result.AddErrorMessage(message);
				Log.Error(message);
			}

			return result;
		}
	}
}
