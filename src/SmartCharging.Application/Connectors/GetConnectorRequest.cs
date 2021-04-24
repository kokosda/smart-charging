using SmartCharging.Application.GeneralRequests;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Application.Connectors
{
	public sealed class GetConnectorRequest : GetIntIdEntityRequest<Connector, ConnectorDto>
	{
	}
}
