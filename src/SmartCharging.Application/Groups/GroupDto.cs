using SmartCharging.Domain.Groups;
using System;
using System.Linq;

namespace SmartCharging.Application.Groups
{
	public sealed record GroupDto
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public decimal CapacityInAmps { get; set; }
		public int[] ChargeStationIds { get; set; }

		public GroupDto()
		{
			ChargeStationIds = new int[0];
		}

		public static GroupDto From(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new GroupDto
			{
				Id = group.Id,
				Name = group.Name,
				CapacityInAmps = group.CapacityInAmps,
				ChargeStationIds = group.ChargeStations.Select(cs => cs.Id).ToArray()
			};
			return result;
		}
	}
}
