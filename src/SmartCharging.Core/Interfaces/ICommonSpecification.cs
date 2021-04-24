using System.Threading.Tasks;

namespace SmartCharging.Core.Interfaces
{
	public interface ICommonSpecification<T>
	{
		Task<IResponseContainer> IsSatisfiedBy(T subject);
	}
}
