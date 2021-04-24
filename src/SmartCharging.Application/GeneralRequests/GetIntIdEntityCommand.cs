using System;
using SmartCharging.Core.Domain;

namespace SmartCharging.Application.GeneralRequests
{
	public sealed record GetIntIdEntityCommand<TEntity, TDto> where TEntity: EntityBase<int>
	{
		public GetIntIdEntityRequest<TEntity, TDto> Request { get; init; }
		public Func<TEntity, TDto> DtoFactory { get; init; }

		public void Deconstruct(out GetIntIdEntityRequest<TEntity, TDto> request, out Func<TEntity, TDto> dtoFactory)
		{
			request = Request;
			dtoFactory = DtoFactory;
		}
	}
}
