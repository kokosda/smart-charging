using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors.Factories;

namespace SmartCharging.Domain.Connectors.Strategies
{
	public interface IConnectorAllocationStrategy
	{
		Task<IResponseContainerWithValue<IReadOnlyList<CreateConnectorSuggestion>>> AllocateAsync(int chargeStationId, decimal maxCurrentInAmps);
	}
}
