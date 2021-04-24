using SmartCharging.Core.Interfaces;
using System.Threading.Tasks;

namespace SmartCharging.Core.Handlers
{
	public interface ICommandHandler<T>
	{
		Task<IResponseContainer> HandleAsync(T command);
	}
}
