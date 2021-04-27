using SmartCharging.Domain.Connectors.Factories;
using System.Collections.Generic;

namespace SmartCharging.Domain.Connectors.Strategies
{
	internal record Suggestion
	{
		private readonly List<int> list;

		public IReadOnlyList<int> List => list;

		public Suggestion(IEnumerable<int> collection)
		{
			list = new List<int>(collection);
		}

		public void AddIndex(int index)
		{
			list.Add(index);
		}

		public void ReplaceIndexValue(int position, int value)
		{
			list[position] = value;
		}

		public override string ToString()
		{
			return $"[{string.Join(",", list)}]";
		}

		public static ConnectorCreationSuggestion ToCreateConnectorSuggestion(Suggestion suggestion, IReadOnlyList<Connector> allConnectors)
		{
			var connectors = new List<Connector>();

			foreach (var connectorIndex in suggestion.list)
			{
				var connector = allConnectors[connectorIndex];
				connectors.Add(connector);
			}

			var result = new ConnectorCreationSuggestion(connectors);
			return result;
		}
	}
}
