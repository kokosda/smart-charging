using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.Groups
{
	public sealed record CreateGroupRequest
	{
		[Required]
		[StringLength(127)]
		public string Name { get; init; }
	}
}
