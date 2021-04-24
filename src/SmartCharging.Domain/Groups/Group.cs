using System;
using System.Collections.Generic;
using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Common;

namespace SmartCharging.Domain.Groups
{
	public sealed class Group : EntityBase<int>
	{
		public Group()
		{
			ChargeStations = new List<ChargeStation>();
		}

		private Group(string name)
		{
			Name = name;
		}

		public Group(string name, decimal capacityInAmps)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			CapacityInAmps = capacityInAmps <= 0 ? throw new ArgumentException($"Current capacity has to be not less than 0 in Amps.") : capacityInAmps;
			ChargeStations = new List<ChargeStation>();
		}

		public string Name { get; init; }
		public decimal CapacityInAmps { get; init; }
		public IList<ChargeStation> ChargeStations { get; init; }

		public static IResponseContainerWithValue<Group> Create(string name)
		{
			IResponseContainerWithValue<Group> result;
			var nameSpecificationResponseContainer = new NameSpecification().IsSatisfiedBy(name).Result;

			if (!nameSpecificationResponseContainer.IsSuccess)
				result = new ResponseContainerWithValue<Group>(). JoinWith(nameSpecificationResponseContainer);
			else
				result = new ResponseContainerWithValue<Group> { Value = new Group(name) };

			return result;
		}
	}
}
