using SmartCharging.Application.Responses;
using SmartCharging.Application.Suggestions;
using SmartCharging.Domain.Connectors.Factories;
using System.Collections.Generic;
using System.Linq;

namespace SmartCharging.Application.Connectors
{
	public sealed record CreateConnectorResponse : ResponseBase
	{
		public ConnectorDto Connector { get; set; }
		public List<ConnectorRemovalSuggestionDto> RemovalSuggestions { get; set; }

		public CreateConnectorResponse()
		{
			RemovalSuggestions = new List<ConnectorRemovalSuggestionDto>();
		}

		public static CreateConnectorResponse From(ConnectorFactoryResult factoryResult)
		{
			var result = new CreateConnectorResponse
			{
				Connector = factoryResult.Connector != null ? ConnectorDto.From(factoryResult.Connector) : null,
				RemovalSuggestions = factoryResult.Suggestions.Select(s => 
					new ConnectorRemovalSuggestionDto 
						{ ConnectorIds = s.ConnectorsToRemove.Select(c => c.Id).ToList() }
				).ToList()
			};

			return result;
		}
	}
}
