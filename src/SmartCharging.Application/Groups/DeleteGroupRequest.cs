using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Application.Groups
{
	public sealed record DeleteGroupRequest
	{
		[Range(1, Int32.MaxValue)]
		public int Id { get; init; }
	}
}
