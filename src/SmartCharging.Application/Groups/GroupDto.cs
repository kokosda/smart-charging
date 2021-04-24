using System;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Application.Groups
{
	public sealed record GroupDto
	{
		public int Id { get; init; }
		public string Name { get; init; }

		internal static GroupDto From(Group group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			var result = new GroupDto
			{
				Id = group.Id,
				Name = group.Name
			};
			return result;
		}
	}
}
