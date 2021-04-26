using System.Collections.Generic;
using System.Linq;
using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Domain.Groups
{
	public sealed class Group : EntityBase<int>
	{
		private readonly List<ChargeStation> chargeStations;

		public Group()
		{
			chargeStations = new List<ChargeStation>();
		}

		private Group(string name, decimal capacityInAmps) : this()
		{
			Name = name;
			CapacityInAmps = capacityInAmps;
		}

		public string Name { get; init; }
		public decimal CapacityInAmps { get; init; }
		public List<ChargeStation> ChargeStations => chargeStations;

		public decimal GetOccupiedCapacity()
		{
			var result = ChargeStations.SelectMany(cs => cs.Connectors).Sum(c => c.MaxCurrentInAmps);
			return result;
		}

		public bool WillBecomeOvercapped(decimal occupiedCapacity, decimal presentedMaxCurrentInAmps, decimal newMaxCurrentInAmps)
		{
			var result = CapacityInAmps < (occupiedCapacity - presentedMaxCurrentInAmps + newMaxCurrentInAmps);
			return result;
		}

		public bool WillBecomeOvercapped(decimal occupiedCapacity, decimal newMaxCurrentInAmps)
		{
			var result = CapacityInAmps < (occupiedCapacity + newMaxCurrentInAmps);
			return result;
		}

		public static IResponseContainerWithValue<Group> Create(string name, decimal capacityInAmps)
		{
			IResponseContainerWithValue<Group> result;
			var group = new Group(name, capacityInAmps);
			var createGroupSpecificationResponseContainer = new CreateGroupSpecification(name, capacityInAmps).IsSatisfiedBy(group).Result;

			if (!createGroupSpecificationResponseContainer.IsSuccess)
				result = new ResponseContainerWithValue<Group>().JoinWith(createGroupSpecificationResponseContainer);
			else
				result = new ResponseContainerWithValue<Group> { Value = group };

			return result;
		}
	}
}
