using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors.Factories
{
	public interface IConnectorFactory
	{
		Task<IResponseContainerWithValue<ConnectorFactoryResult>> CreateAsync(int chargeStationId, int lineNo, decimal maxCurrentInAmps);
	}
}
