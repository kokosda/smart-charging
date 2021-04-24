using System;
using System.Threading.Tasks;
using SmartCharging.Application.Handlers;
using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.GeneralRequests
{
	public sealed class GetIntIdEntityHandler<TEntity, TDto> : GenericCommandHandlerBase<GetIntIdEntityCommand<TEntity, TDto>, TDto>, IGetIntIdEntityHandler<TEntity, TDto> where TEntity : EntityBase<int>
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(GetIntIdEntityHandler<TEntity, TDto>));
		private readonly IGenericRepository<TEntity, int> genericRepository;

		public GetIntIdEntityHandler(IGenericRepository<TEntity, int> genericRepository)
		{
			this.genericRepository = genericRepository;
		}

		protected override async Task<IResponseContainerWithValue<TDto>> GetResultAsync(GetIntIdEntityCommand<TEntity, TDto> command)
		{
			if (command.DtoFactory is null)
				throw new ArgumentException($"DTO factory property is not assigned.");

			IResponseContainerWithValue<TDto> result;
			var (request, dtoFactory) = command;
			var entity = await genericRepository.GetAsync(request.Id);

			if (entity is null)
			{
				result = new ResponseContainerWithValue<TDto>();
				result.AddErrorMessage($"{typeof(TEntity).Name} with ID={request.Id} is not found.");
				Log.Error(result.Messages);
			}
			else
				result = new ResponseContainerWithValue<TDto> { Value = dtoFactory(entity) };

			return result;
		}
	}
}
