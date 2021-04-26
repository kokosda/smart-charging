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

		[TestCase(TestName = "Adding new connector exceeds group's capacity. Several removal suggestions are returned.")]
		public void AllocateAsync_AddingNewConnectorExceedsCapacity_ReturnsResponseWithSuggestions()
		{
			// Arrange
			var chargeStationId = 12;
			var group = GetGroup(chargeStationId);
			var connectors = group.ChargeStations.SelectMany(cs => cs.Connectors).OrderByDescending(c => c.MaxCurrentInAmps).ToList();

			groupRepositoryMock.Setup(gr => gr.GetByChargeStationIdAsync(chargeStationId)).Returns(Task.FromResult(group));
			connectorRepositoryMock.Setup(cr => cr.GetAllInGroupByChargeStationIdAsync(chargeStationId))
				.Returns(Task.FromResult((IReadOnlyList<Connector>) connectors));

			// Act
			var result = strategy.AllocateAsync(chargeStationId, 15).Result;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value.Count, Is.GreaterThan(1));
		}

		private Group GetGroup(int chargeStationId)
		{
			var result = new Group
			{
				Id = 7,
				Name = "Tesla XX Groups 2025",
				CapacityInAmps = 40
			};

			var chargeStation = new ChargeStation { Name = "LA Based Tesla 01" };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 2 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Tokyo Nissan 18" };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 9 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 5 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 1 });
			result.ChargeStations.Add(chargeStation);

			chargeStation = new ChargeStation { Name = "Frankfurt Audi 3" };
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 3 });
			chargeStation.Connectors.Add(new Connector { MaxCurrentInAmps = 7 });
			result.ChargeStations.Add(chargeStation);

			return result;
		}
	}
}