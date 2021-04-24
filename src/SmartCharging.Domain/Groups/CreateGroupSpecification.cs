using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Common;

namespace SmartCharging.Domain.Groups
{
	public sealed class CreateGroupSpecification : ISpecification<Group, int>
	{
		private readonly decimal capacityInAmps;
		private readonly string name;

		public CreateGroupSpecification(string name, decimal capacityInAmps)
		{
			this.capacityInAmps = capacityInAmps;
			this.name = name;
		}

		public Task<IResponseContainer> IsSatisfiedBy(Group entity)
		{
			var result = new ResponseContainer();

			if (capacityInAmps <= 0)
				result.AddErrorMessage($"Capacity has to be greater than 0.");

			var nameSpecificationResponseContainer = new NameSpecification().IsSatisfiedBy(name).Result;
			result.JoinWith(nameSpecificationResponseContainer);

			return Task.FromResult(result.AsInterface());
		}
	}
}
