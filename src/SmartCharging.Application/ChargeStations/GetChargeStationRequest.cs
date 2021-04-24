using SmartCharging.Application.GeneralRequests;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Application.ChargeStations
{
	public sealed class GetChargeStationRequest : GetIntIdEntityRequest<ChargeStation, ChargeStationDto>
	{
	}
}
