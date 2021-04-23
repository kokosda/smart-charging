using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public interface IUpdateConnectorHandler
	{
		Task<IResponseContainer> UpdateMaxCurrentAsync(UpdateConnectorRequest request);
	}
}
