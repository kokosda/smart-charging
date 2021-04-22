using System;
using System.Collections.Generic;
using SmartCharging.Core;
using SmartCharging.Core.Entities;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Domain.Groups
{
	public sealed class Group : EntityBase<int>
	{
		public Group(string name, decimal capacityInAmps)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			CapacityInAmps = capacityInAmps <= 0 ? throw new ArgumentException($"Current capacity has to be not less than 0 in Amps.") : capacityInAmps;
			ChargeStations = new List<ChargeStation>();
		}

		public string Name { get; private set; }
		public decimal CapacityInAmps { get; private set; }
		public IList<ChargeStation> ChargeStations { get; private set; }
	}
}
