using System.Collections.Generic;
using System.Linq;
using SmartCharging.Core.Domain;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed record ConnectorCreationSuggestion : ValueObject
	{
		private readonly List<Connector> connectorsToRemove;

		public IReadOnlyList<Connector> ConnectorsToRemove => connectorsToRemove;

		public ConnectorCreationSuggestion(IEnumerable<Connector> connectors)
		{
			connectorsToRemove = new List<Connector>(connectors);
		}

		public override string ToString()
		{
			return $"[{string.Join(",", connectorsToRemove.Select(c => new { c.Id, c.MaxCurrentInAmps }.ToString()))}]";
		}
	}
}
