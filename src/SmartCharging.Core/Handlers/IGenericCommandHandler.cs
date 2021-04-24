using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Core.Handlers
{
	public interface IGenericCommandHandler<TCommand, TResult> : ICommandHandler<TCommand>
	{
		Task<IResponseContainerWithValue<TResult>> HandleWithValueAsync(TCommand command);
	}
}
