using System.Collections.Generic;
using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Domain.Groups
{
	public sealed class Group : EntityBase<int>
	{
		public Group()
		{
			ChargeStations = new List<ChargeStation>();
		}

		private Group(string name, decimal capacityInAmps) : this()
		{
			Name = name;
			CapacityInAmps = capacityInAmps;
		}

		public string Name { get; init; }
		public decimal CapacityInAmps { get; init; }
		public IList<ChargeStation> ChargeStations { get; init; }

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
