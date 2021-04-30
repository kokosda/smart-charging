using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.ChargeStations
{
	public sealed record DeleteChargeStationRequest
	{
		[Range(1, int.MaxValue)]
		public int Id { get; init; }
	}
}