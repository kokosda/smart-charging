using System;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Application.Connectors
{
	public sealed record ConnectorDto
	{
		public int LineNo { get; init; }
		public int ChargeStationId { get; init; }
		public decimal MaxCurrentInAmps { get; init; }

		public static ConnectorDto From(Connector connector)
		{
			if (connector is null)
				throw new ArgumentNullException(nameof(connector));

			var result = new ConnectorDto
			{
				LineNo = connector.LineNo,
				ChargeStationId = connector.ChargeStationId,
				MaxCurrentInAmps = connector.MaxCurrentInAmps
			};
			return result;
		}
	}
}
