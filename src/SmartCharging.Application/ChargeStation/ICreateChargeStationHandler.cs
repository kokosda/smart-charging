using SmartCharging.Core.Handlers;
using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Application.Connectors
{
	public interface ICreateChargeStationHandler : ICommandHandler<CreateChargeStationRequest>
	{
	}
}
