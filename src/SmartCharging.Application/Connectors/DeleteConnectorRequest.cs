using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.Connectors
{
	public sealed record DeleteConnectorRequest
	{
		[Range(1, int.MaxValue)]
		public int ChargeStationId { get; init; }

		[Range(1, 5)]
		public int LineNo { get; init; }
	}
}
