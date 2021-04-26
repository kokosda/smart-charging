using SmartCharging.Core.Handlers;

namespace SmartCharging.Application.Connectors
{
	public interface ICreateConnectorHandler : IGenericCommandHandler<CreateConnectorRequest, CreateConnectorResponse>
	{
	}
}
