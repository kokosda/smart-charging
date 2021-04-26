using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Domain.Groups
{
	public interface IGroupRepository : IGenericRepository<Group, int>
	{
		Task<Group> GetByConnectorAsync(Connector connector);
		Task<Group> GetByChargeStationIdAsync(int chargeStationId);
	}
}
