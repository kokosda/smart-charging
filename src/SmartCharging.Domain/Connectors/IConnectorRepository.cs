using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Domain.Connectors
{
	public interface IConnectorRepository : IGenericRepository<Connector, int>
	{
		Task<Connector> GetByChargeStationIdAndLineNo(int chargeStationId, int lineNo);
	}
}
