using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Connectors.Factories;
using SmartCharging.Domain.Groups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors.Strategies
{
	public sealed class AllocationThroughMinNumberExclusionStrategy : IConnectorAllocationStrategy
	{
		private readonly IConnectorRepository connectorRepository;
		private readonly IGroupRepository groupRepository;

		public AllocationThroughMinNumberExclusionStrategy(IConnectorRepository connectorRepository, IGroupRepository groupRepository)
		{
			this.connectorRepository = connectorRepository;
			this.groupRepository = groupRepository;
		}

		public async Task<IResponseContainerWithValue<IReadOnlyList<ConnectorCreationSuggestion>>> AllocateAsync(int chargeStationId, decimal maxCurrentInAmps)
		{
			IResponseContainerWithValue<IReadOnlyList<ConnectorCreationSuggestion>> result;

			var group = await groupRepository.GetByChargeStationIdAsync(chargeStationId);
			var createConnectorSuggestions = new List<ConnectorCreationSuggestion>();

			if (group.CapacityInAmps < maxCurrentInAmps)
			{
				result = new ResponseContainerWithValue<IReadOnlyList<ConnectorCreationSuggestion>>();
				result.AddErrorMessage($"Max current {maxCurrentInAmps} provided exceeds group's {group.Name} capacity.");
				return result;
			}

			var occupiedCapacity = group.GetOccupiedCapacity();

			if (!group.WillBecomeOvercapped(occupiedCapacity, maxCurrentInAmps))
				return new ResponseContainerWithValue<IReadOnlyList<ConnectorCreationSuggestion>> { Value = createConnectorSuggestions };

			var connectors = await connectorRepository.GetAllInGroupByChargeStationIdAsync(chargeStationId);
			var suggestions = GetUniqueSuggestions(group, connectors, occupiedCapacity, maxCurrentInAmps);
			var suggestionCombinations = GetIndexesCombinations(suggestions, connectors);

			suggestions.AddRange(suggestionCombinations);
			createConnectorSuggestions.AddRange(suggestions.Select(s => Suggestion.ToCreateConnectorSuggestion(s, connectors)));

			result = new ResponseContainerWithValue<IReadOnlyList<ConnectorCreationSuggestion>> { Value = createConnectorSuggestions };
			return result;
		}

		private List<Suggestion> GetUniqueSuggestions(
			Group group, 
			IReadOnlyList<Connector> connectors,
			decimal occupiedCapacity, 
			decimal maxCurrentInAmps
		)
		{

			var availableCapacity = group.CapacityInAmps - occupiedCapacity;
			var freeingCapacityConst = maxCurrentInAmps - availableCapacity;
			var lastSuggestionCount = connectors.Count;
			var i = 0;
			var suggestions = new List<Suggestion>();

			while (i < connectors.Count)
			{
				if (connectors[i].MaxCurrentInAmps <= maxCurrentInAmps)
				{
					var pos = i;
					var freeingCapacity = freeingCapacityConst - connectors[pos].MaxCurrentInAmps;
					var suggestion = new Suggestion(new[] { pos });

					while (freeingCapacity > 0M && suggestion.List.Count < lastSuggestionCount)
					{
						pos = SearchBinaryInDescendingList(connectors, freeingCapacity, pos);

						if (pos < connectors.Count && (freeingCapacity - connectors[pos].MaxCurrentInAmps >= 0M))
						{
							suggestion.AddIndex(pos);
							freeingCapacity -= connectors[pos].MaxCurrentInAmps;
						}
						else
							break;
					}

					if (freeingCapacity == 0M && suggestion.List.Count <= lastSuggestionCount)
					{
						lastSuggestionCount = suggestion.List.Count;
						suggestions.Add(suggestion);
					}
				}

				i++;
			}

			var result = new List<Suggestion>();

			foreach (var suggestion in suggestions)
			{
				if (suggestion.List.Count > lastSuggestionCount)
					continue;

				result.Add(suggestion);
			}

			return result;
		}

		private static IReadOnlyList<Suggestion> GetIndexesCombinations(IEnumerable<Suggestion> suggestions, IReadOnlyList<Connector> connectors)
		{
			var result = new List<Suggestion>();

			foreach (var suggestion in suggestions)
			{
				for (var indexInSuggestion = 0; indexInSuggestion < suggestion.List.Count; indexInSuggestion++)
				{
					var indexInConnectors = suggestion.List[indexInSuggestion];
					var lastIndexInConnectors = connectors.Count;

					if (indexInSuggestion + 1 < suggestion.List.Count)
						lastIndexInConnectors = suggestion.List[indexInSuggestion + 1];

					var i = indexInConnectors + 1;

					while (i < lastIndexInConnectors && connectors[i].MaxCurrentInAmps == connectors[indexInConnectors].MaxCurrentInAmps)
					{
						var suggestionCombination = new Suggestion(suggestion.List);
						suggestionCombination.ReplaceIndexValue(indexInSuggestion, i);
						result.Add(suggestionCombination);
						i++;
					}
				}
			}

			return result;
		}

		private static int SearchBinaryInDescendingList(IReadOnlyList<Connector> connectors, decimal current, int loIndex)
		{
			var left = loIndex;
			var right = connectors.Count;

			while (right - left > 1)
			{
				var mid = (left + right) >> 1;

				if (connectors[mid].MaxCurrentInAmps > current)
					left = mid;
				else
					right = mid;
			}

			return right;
		}
	}
}
