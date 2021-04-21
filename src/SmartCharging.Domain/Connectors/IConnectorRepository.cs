using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors
{
	public interface IConnectorRepository
	{
		Task<Connector> GetByChargeStationIdAndLineNo(int chargeStationId, int lineNo);
	}
}
