using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.ChargeStations
{
	public sealed record CreateChargeStationRequest
	{
		[Required]
		[StringLength(127)]
		public string Name { get; init; }

		[Range(1, int.MaxValue)]
		public int GroupId { get; init; }
	}
}
