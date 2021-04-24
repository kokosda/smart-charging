using SmartCharging.Core.Handlers;

namespace SmartCharging.Application.ChargeStations
{
	public interface ICreateChargeStationHandler : IGenericCommandHandler<CreateChargeStationRequest, ChargeStationDto>
	{
	}
}
