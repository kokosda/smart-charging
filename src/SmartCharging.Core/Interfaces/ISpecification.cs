using SmartCharging.Core.Domain;

namespace SmartCharging.Core.Interfaces
{
	public interface ISpecification<T, TId> where T: EntityBase<TId>
	{
		bool IsSatisfiedBy(T entity);
	}
}
