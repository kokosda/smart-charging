using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Domain.Connectors
{
	public interface IConnectorRepository : IGenericRepository<Connector, int>
	{
		Task<Connector> GetByChargeStationIdAndLineNoAsync(int chargeStationId, int lineNo);
		Task<IReadOnlyList<Connector>> GetAllInGroupByChargeStationIdAsync(int chargeStationId);
	}
}
