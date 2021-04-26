using System.Collections.Generic;

namespace SmartCharging.Application.Suggestions
{
	public sealed class ConnectorRemovalSuggestionDto
	{
		public List<int> ConnectorIds { get; init; }

		public ConnectorRemovalSuggestionDto()
		{
			ConnectorIds = new List<int>();
		}
	}
}
