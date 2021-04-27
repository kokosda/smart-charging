using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Connectors.Strategies;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Domain.Tests.Connectors.Strategies
{
	[TestFixture]
	public sealed class AllocationThroughMinNumberExclusionStrategyTests
	{
		private AllocationThroughMinNumberExclusionStrategy strategy;
		private MockRepository mockRepository;
		private Mock<IConnectorRepository> connectorRepositoryMock;
		private Mock<IGroupRepository> groupRepositoryMock;

		[SetUp]
		public void Setup()
		{
			mockRepository = new MockRepository(MockBehavior.Strict);
			connectorRepositoryMock = mockRepository.Create<IConnectorRepository>();
			groupRepositoryMock = mockRepository.Create<IGroupRepository>();
			strategy = new AllocationThroughMinNumberExclusionStrategy(connectorRepositoryMock.Object, groupRepositoryMock.Object);
		}

		[TestCase(TestName = "Adding new connector exceeds group's capacity. Several removal suggestions are returned. MaxCurrentInAmps: [[16,9, 7, 5, 5, 3, 3, 2, 2, 1, 1]]")]
		public void AllocateAsync_AddingNewConnectorExceedsCapacity_ReturnsResponseWithSuggestions1()
		{
			// Arrange
			var chargeStationId = 12;
			var group = GetGroup1();
			var connectors = group.ChargeStations.SelectMany(cs => cs.Connectors).OrderByDescending(c => c.MaxCurrentInAmps).ToList();

			var expectedSuggestions = new List<int[]>
			{
				new [] { 5, 1, 8 },
				new [] { 11, 2, 8 },
				new [] { 2, 6, 1 },
				new [] { 5, 10, 8 },
				new [] { 5, 1, 9 },
				new [] { 11, 6, 8 },
				new [] { 11, 2, 9 },
				new [] { 2, 6, 10 },
			};

			groupRepositoryMock.Setup(gr => gr.GetByChargeStationIdAsync(chargeStationId)).Returns(Task.FromResult(group));
			connectorRepositoryMock.Setup(cr => cr.GetAllInGroupByChargeStationIdAsync(chargeStationId))
				.Returns(Task.FromResult((IReadOnlyList<Connector>) connectors));

			// Act
			var result = strategy.AllocateAsync(chargeStationId, 15).Result;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);

			foreach (var suggestion in result.Value)
			{
				var connectorIdsHashSet = suggestion.ConnectorsToRemove.Select(c => c.Id).ToHashSet();
				Assert.That(expectedSuggestions.Any(es => connectorIdsHashSet.SetEquals(es)));
			}
		}

		[TestCase(TestName = "Adding new connector exceeds group's capacity. Several removal suggestions are returned. MaxCurrentInAmps: [7, 7, 5, 5, 3, 3, 2, 2, 1, 1]")]
		public void AllocateAsync_AddingNewConnectorExceedsCapacity_ReturnsResponseWithSuggestions2()
		{
			// Arrange
			var chargeStationId = 12;
			var group = GetGroup2();
			var connectors = group.ChargeStations.SelectMany(cs => cs.Connectors).OrderByDescending(c => c.MaxCurrentInAmps).ToList();

			var expectedSuggestions = new List<int[]>
			{
				new [] { 5, 11 }
			};

			groupRepositoryMock.Setup(gr => gr.GetByChargeStationIdAsync(chargeStationId)).Returns(Task.FromResult(group));
			connectorRepositoryMock.Setup(cr => cr.GetAllInGroupByChargeStationIdAsync(chargeStationId))
				.Returns(Task.FromResult((IReadOnlyList<Connector>)connectors));

			// Act
			var result = strategy.AllocateAsync(chargeStationId, 15).Result;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);

			foreach (var suggestion in result.Value)
			{
				var connectorIdsHashSet = suggestion.ConnectorsToRemove.Select(c => c.Id).ToHashSet();
				Assert.That(expectedSuggestions.Any(es => connectorIdsHashSet.SetEquals(es)));
			}
		}

		[TestCase(TestName = "Group's capacity encompasses new connector. Empty removal suggestion list returned.")]
		public void AllocateAsync_GroupsCapacityEncompassesNewConnector_ReturnsResponseWithSuggestions()
		{
			// Arrange
			var chargeStationId = 12;
			var group = GetGroup3();
			var connectors = group.ChargeStations.SelectMany(cs => cs.Connectors).OrderByDescending(c => c.MaxCurrentInAmps).ToList();

			groupRepositoryMock.Setup(gr => gr.GetByChargeStationIdAsync(chargeStationId)).Returns(Task.FromResult(group));
			connectorRepositoryMock.Setup(cr => cr.GetAllInGroupByChargeStationIdAsync(chargeStationId))
				.Returns(Task.FromResult((IReadOnlyList<Connector>)connectors));

			// Act
			var result = strategy.AllocateAsync(chargeStationId, 15).Result;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value.Count, Is.Zero);
		}

		[TestCase(TestName = "New connector's max current exceeds group's capacity. Error-contained response returned.")]
		public void AllocateAsync_NewConnectorMaxCurrentExceedsGroupsCapacity_ReturnsErrorContainedResponse()
		{
			// Arrange
			var chargeStationId = 12;
			var group = new Group { CapacityInAmps = 8M };

			groupRepositoryMock.Setup(gr => gr.GetByChargeStationIdAsync(chargeStationId)).Returns(Task.FromResult(group));

			// Act
			var result = strategy.AllocateAsync(chargeStationId, 15M).Result;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Null);
			Assert.That(result.IsSuccess, Is.False);
			Assert.That(result.Messages, Is.Not.Null);
		}

		private Group GetGroup1()
		{
			var result = new Group
			{
				Id = 7,
				Name = "Tesla XX Groups 2025",
				CapacityInAmps = 56
			};

			var chargeStation = new ChargeStation { Name = "LA Based Tesla 01", Id = 1 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3, LineNo = 1, ChargeStationId = 1, Id = 1 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 1, Id = 2 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2, LineNo = 3, ChargeStationId = 1, Id = 3 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2, LineNo = 4, ChargeStationId = 1, Id = 4 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Tokyo Nissan 18", Id = 2 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 9, LineNo = 1, ChargeStationId = 2, Id = 5 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 2, Id = 6 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 16, LineNo = 3, ChargeStationId = 2, Id = 7 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1, LineNo = 4, ChargeStationId = 2, Id = 8 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1, LineNo = 5, ChargeStationId = 2, Id = 9 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Frankfurt Audi 3", Id = 3 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3, LineNo = 1, ChargeStationId = 3, Id = 10 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7, LineNo = 2, ChargeStationId = 3, Id = 11 });
			result.ChargeStations.Add(chargeStation);

			return result;
		}

		private Group GetGroup2()
		{
			var result = new Group
			{
				Id = 7,
				Name = "Tesla XX Groups 2025",
				CapacityInAmps = 37
			};

			var chargeStation = new ChargeStation { Name = "LA Based Tesla 01", Id = 1 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3, LineNo = 1, ChargeStationId = 1, Id = 1 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 1, Id = 2 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2, LineNo = 3, ChargeStationId = 1, Id = 3 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2, LineNo = 4, ChargeStationId = 1, Id = 4 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Tokyo Nissan 18", Id = 2 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7, LineNo = 1, ChargeStationId = 2, Id = 5 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 2, Id = 6 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1, LineNo = 4, ChargeStationId = 2, Id = 8 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1, LineNo = 5, ChargeStationId = 2, Id = 9 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Frankfurt Audi 3", Id = 3 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3, LineNo = 1, ChargeStationId = 3, Id = 10 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7, LineNo = 2, ChargeStationId = 3, Id = 11 });
			result.ChargeStations.Add(chargeStation);

			return result;
		}

		private Group GetGroup3()
		{
			var result = new Group
			{
				Id = 7,
				Name = "Tesla XX Groups 2025",
				CapacityInAmps = 94
			};

			var chargeStation = new ChargeStation { Name = "LA Based Tesla 01", Id = 1 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3, LineNo = 1, ChargeStationId = 1, Id = 1 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 1, Id = 2 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Tokyo Nissan 18", Id = 2 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7, LineNo = 1, ChargeStationId = 2, Id = 5 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5, LineNo = 2, ChargeStationId = 2, Id = 6 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Frankfurt Audi 3", Id = 3 };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7, LineNo = 2, ChargeStationId = 3, Id = 11 });
			result.ChargeStations.Add(chargeStation);

			return result;
		}
	}
}