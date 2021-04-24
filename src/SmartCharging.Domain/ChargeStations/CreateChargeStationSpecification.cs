using System;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.ChargeStations
{
	public sealed record CreateChargeStationSpecification : ISpecification<ChargeStation, int>
	{
		private readonly Group group;
		private readonly string name;

		public CreateChargeStationSpecification(Group group, string name)
		{
			this.group = group ?? throw new ArgumentNullException(nameof(group));
			this.name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public async Task<IResponseContainer> IsSatisfiedBy(ChargeStation chargeStation)
		{
			if (chargeStation is null)
				throw new ArgumentNullException(nameof(chargeStation));

			var result = new ResponseContainer();

			if (name)

			return result;
		}
	}
}
