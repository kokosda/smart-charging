using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.Connectors
{
	public sealed class CreateConnectorRequest
	{
		[Range(1, int.MaxValue)]
		public int ChargeStationId { get; init; }

		[Range(1, 5)]
		public int LineNo { get; init; }

		[Range(0.000001, double.MaxValue)]
		public decimal MaxCurrentInAmps { get; init; }
	}
}
