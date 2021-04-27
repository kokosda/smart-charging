using System.Collections.Generic;
using SmartCharging.Core.Domain;

namespace SmartCharging.Domain.Connectors.Factories
{
	public sealed record ConnectorFactoryResult : ValueObject
	{
		private readonly List<ConnectorCreationSuggestion> suggestions;

		public Connector Connector { get; init; }
		public IReadOnlyList<ConnectorCreationSuggestion> Suggestions { get => suggestions; }

		public ConnectorFactoryResult(IEnumerable<ConnectorCreationSuggestion> suggestions)
		{
			this.suggestions = new List<ConnectorCreationSuggestion>(suggestions);
		}
	}
}
