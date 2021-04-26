using System.Collections.Generic;
using SmartCharging.Core.Domain;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed record ConnectorFactoryResult : ValueObject
	{
		private readonly List<CreateConnectorSuggestion> suggestions;

		public Connector Connector { get; init; }
		public IReadOnlyList<CreateConnectorSuggestion> Suggestions { get => suggestions; }

		public ConnectorFactoryResult(IEnumerable<CreateConnectorSuggestion> suggestions)
		{
			this.suggestions = new List<CreateConnectorSuggestion>(suggestions);
		}
	}
}
