using System;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Domain.Connectors
{
	public sealed record UpdateMaxCurrentSpecification : ISpecification<Connector, int>
	{
		public decimal Current { get; init; }

		public bool IsSatisfiedBy(Connector connector)
		{
			if (connector is null)
				throw new ArgumentNullException(nameof(connector));

			if (Current <= 0)
				return false;

			return true;
		}
	}
}
