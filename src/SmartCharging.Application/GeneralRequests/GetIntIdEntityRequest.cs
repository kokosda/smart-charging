using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartCharging.Application.GeneralRequests
{
	[Serializable]
	public class GetIntIdEntityRequest<TEntity, TDto>
	{
		[Range(1, int.MaxValue)]
		public int Id { get; init; }
	}
}
