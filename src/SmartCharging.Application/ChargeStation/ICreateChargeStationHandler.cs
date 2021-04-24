using SmartCharging.Core.Handlers;

namespace SmartCharging.Application.Connectors
{
	public interface ICreateChargeStationHandler : IGenericCommandHandler<CreateChargeStationRequest, ChargeStationDto>
	{
	}
}
