using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Core.Handlers
{
	public interface IGenericCommandHandler<TCommand, TResult>
	{
		Task<IResponseContainerWithValue<TResult>> HandleAsync(TCommand command);
	}
}
