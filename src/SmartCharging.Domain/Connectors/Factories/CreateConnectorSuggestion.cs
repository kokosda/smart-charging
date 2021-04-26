using System.Collections.Generic;
using SmartCharging.Core.Domain;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed record CreateConnectorSuggestion : ValueObject
	{
		private readonly IReadOnlyList<Connector> connectorsToRemove;

		public IReadOnlyList<Connector> ConnectorsToRemove { get => connectorsToRemove; init => connectorsToRemove = value; }

		public CreateConnectorSuggestion(IEnumerable<Connector> connectors)
		{
			connectorsToRemove = new List<Connector>(connectors);
		}
	}
}
