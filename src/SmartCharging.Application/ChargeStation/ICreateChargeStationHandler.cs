using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public interface ICreateChargeStationHandler
	{
		Task<IResponseContainer> CreateChargeStationAsync(CreateChargeStationRequest request);
	}
}
