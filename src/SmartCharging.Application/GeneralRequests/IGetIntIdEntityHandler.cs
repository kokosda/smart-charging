using SmartCharging.Core.Domain;
using SmartCharging.Core.Handlers;

namespace SmartCharging.Application.GeneralRequests
{
	public interface IGetIntIdEntityHandler<TEntity, TDto> : IGenericCommandHandler<GetIntIdEntityCommand<TEntity, TDto>, TDto> where TEntity : EntityBase<int>
	{
	}
}
