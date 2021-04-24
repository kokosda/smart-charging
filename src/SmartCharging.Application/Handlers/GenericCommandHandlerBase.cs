using SmartCharging.Core.Handlers;
using SmartCharging.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace SmartCharging.Application.Handlers
{
	public abstract class GenericCommandHandlerBase<TCommand, TResult> : IGenericCommandHandler<TCommand, TResult>
	{
		public Task<IResponseContainer> HandleAsync(TCommand command)
		{
			throw new NotSupportedException($"Operation {nameof(HandleAsync)} is not supported.");
		}

		public async Task<IResponseContainerWithValue<TResult>> HandleWithValueAsync(TCommand command)
		{
			if (command is null)
				throw new ArgumentNullException(nameof(command));

			return await GetResultAsync(command);
		}

		protected abstract Task<IResponseContainerWithValue<TResult>> GetResultAsync(TCommand command);
	}
}
