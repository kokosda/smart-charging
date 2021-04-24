using System;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Application.Groups
{
	public sealed record GroupDto
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public decimal CapacityInAmps { get; set; }

		public static GroupDto From(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new GroupDto
			{
				Id = group.Id,
				Name = group.Name,
				CapacityInAmps = group.CapacityInAmps
			};
			return result;
		}
	}
}
