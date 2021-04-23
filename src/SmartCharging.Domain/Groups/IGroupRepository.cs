using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.Connectors
{
	public interface IGroupRepository : IGenericRepository<Group, int>
	{
		Task<Group> GetByConnector(Connector connector);
	}
}
