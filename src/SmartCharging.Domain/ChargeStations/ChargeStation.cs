using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Common;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;
using System;
using System.Collections.Generic;

namespace SmartCharging.Domain.ChargeStations
{
	public sealed class ChargeStation : EntityBase<int>
	{
		public ChargeStation()
		{
			Connectors = new List<Connector>();
		}

		private ChargeStation(string name, int groupId) : this()
		{
			Name = name;
			GroupId = groupId;
		}

		public string Name { get; init; }
		public int GroupId { get; init; }
		public List<Connector> Connectors { get; }

		public static IResponseContainerWithValue<ChargeStation> Create(Group group, string name)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			IResponseContainerWithValue<ChargeStation> result;
			var nameSpecificationResponseContainer = new NameSpecification().IsSatisfiedBy(name).Result;

			if (!nameSpecificationResponseContainer.IsSuccess)
				result = new ResponseContainerWithValue<ChargeStation>().JoinWith(nameSpecificationResponseContainer);
			else
				result = new ResponseContainerWithValue<ChargeStation> { Value = new ChargeStation(name, group.Id) };

			return result;
		}
	}
}
