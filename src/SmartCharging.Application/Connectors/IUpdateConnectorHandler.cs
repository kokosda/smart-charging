using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public interface IUpdateConnectorHandler
	{
		Task UpdateMaxCurrentAsync(UpdateConnectorRequest request);
	}
}
