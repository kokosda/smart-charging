using SmartCharging.Core.Domain;
using System.Threading.Tasks;

namespace SmartCharging.Core.Interfaces
{
	public interface ISpecification<T, TId> where T: EntityBase<TId>
	{
		Task<IResponseContainer> IsSatisfiedBy(T entity);
	}
}
