using System;
using System.Collections.Generic;
using SmartCharging.Core.Entities;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Domain.ChargeStations
{
	public sealed class ChargeStation : EntityBase<int>
	{
		public ChargeStation(string name, int groupId)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			GroupId = groupId;
			Connectors = new List<Connector>();
		}

		public string Name { get; init; }
		public int GroupId { get; init; }
		public IList<Connector> Connectors { get; init; }
	}
}
