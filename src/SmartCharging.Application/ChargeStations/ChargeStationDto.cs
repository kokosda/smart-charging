using System;
using System.Linq;
using SmartCharging.Application.Connectors;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Application.ChargeStations
{
	public sealed record ChargeStationDto
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public int GroupId { get; init; }
		public ConnectorDto[] Connectors { get; init; }

		public ChargeStationDto()
		{
			Connectors = new ConnectorDto[0];
		}

		public static ChargeStationDto From(ChargeStation chargeStation)
		{
			if (chargeStation is null)
				throw new ArgumentNullException(nameof(chargeStation));

			var result = new ChargeStationDto
			{
				Id = chargeStation.Id,
				GroupId = chargeStation.GroupId,
				Name = chargeStation.Name,
				Connectors = chargeStation.Connectors.Select(ConnectorDto.From).ToArray()
			};
			return result;
		}
	}
}
