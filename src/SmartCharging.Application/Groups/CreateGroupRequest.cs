using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.Groups
{
	public sealed record CreateGroupRequest
	{
		[Required]
		[StringLength(127)]
		public string Name { get; init; }

		[Range(0.000001, double.MaxValue)]
		public decimal CapacityInAmps { get; init; }
	}
}
