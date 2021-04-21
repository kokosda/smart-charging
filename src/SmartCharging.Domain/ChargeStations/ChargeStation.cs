using System;
using System.Collections.Generic;
using SmartCharging.Core;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.ChargeStations
{
	public sealed class ChargeStation : EntityBase<int>
	{
		public ChargeStation(string name, Group group)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Group = group ?? throw new ArgumentNullException(nameof(group));
			Connectors = new List<Connector>();
		}

		public string Name { get; init; }
		public Group Group { get; init; }
		public IList<Connector> Connectors { get; init; }
	}
}
