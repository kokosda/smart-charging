using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors.Factories;
using SmartCharging.Infrastructure.Logging;
using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public sealed class CreateConnectorHandler : GenericCommandHandlerBase<CreateConnectorRequest, CreateConnectorResponse>, ICreateConnectorHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(UpdateMaxCurrentConnectorHandler));
		private readonly IConnectorFactory connectorFactory;

		public CreateConnectorHandler(IConnectorFactory connectorFactory)
		{
			this.connectorFactory = connectorFactory;
		}

		protected override async Task<IResponseContainerWithValue<CreateConnectorResponse>> GetResultAsync(CreateConnectorRequest request)
		{
			var factoryResultResponseContainer = await connectorFactory.CreateAsync(request.ChargeStationId, request.LineNo, request.MaxCurrentInAmps);

			if (!factoryResultResponseContainer.IsSuccess)
				Log.Error(factoryResultResponseContainer.Messages);


			var result = new ResponseContainerWithValue<CreateConnectorResponse>
			{
				Value = CreateConnectorResponse.From(factoryResultResponseContainer.Value)
			};
			return result;
		}
	}
}
