using Dapper;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Database;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SmartCharging.Infrastructure.Domain
{
	public sealed class GroupRepository : GenericRepository<Group, int>, IGroupRepository
	{
		public GroupRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
		{
		}

		public override async Task<Group> GetAsync(int id)
		{
			var sql = @$"
select g.* 
from [Group] AS g
where g.Id={id};

select cs.*
from [ChargeStation] AS cs
where cs.GroupId = {id};

select c.*
from [Connector] AS c
join [ChargeStation] AS cs
where cs.GroupId = {id}
";

			var result = await ComposeGroupFromMultipleQuery(sql);
			return result;
		}

		public override async Task DeleteAsync(int id)
		{
			var sql = $@"
delete from [Connector]
where Id in (
	select c.Id
	from [Connector] c
	inner join [ChargeStation] cs on cs.GroupId = {id}
);

delete from [ChargeStation]
where GroupId = {id};

delete from [Group]
where Id = {id};
";

			await sqlConnectionFactory.GetOpenConnection().ExecuteAsync(sql);
		}

		public async Task<Group> GetByChargeStationIdAsync(int chargeStationId)
		{
			var sql = $@"
select g.* 
from [Group] AS g
join [ChargeStation] AS cs on cs.GroupId = g.Id
where cs.Id = {chargeStationId};

select cs.*
from [ChargeStation] AS cs
where cs.GroupId in (
	select g.Id 
	from [Group] AS g 
	join [ChargeStation] AS cs on cs.GroupId = g.Id
	where cs.Id = {chargeStationId}
);

select c.*
from [Connector] AS c
join (
	select cs.Id
	from [ChargeStation] AS cs
	where cs.GroupId in (
		select g.Id 
		from [Group] AS g 
		join [ChargeStation] AS cs on cs.GroupId = g.Id
		where cs.Id = {chargeStationId}
	)
) AS cs on cs.Id = c.ChargeStationId;
";
			var result = await ComposeGroupFromMultipleQuery(sql);
			return result;
		}

		public async Task<Group> GetByConnectorAsync(Connector connector)
		{
			var sql = @$"
select g.* 
from [Group] AS g
join [ChargeStation] AS cs on cs.GroupId = g.Id
join [Connector] AS c on c.ChargeStationId = cs.Id
where c.{nameof(connector.ChargeStationId)}={connector.ChargeStationId} and c.{nameof(connector.LineNo)}={connector.LineNo};

select cs.*
from [ChargeStation] AS cs
where cs.GroupId in (
	select g.Id 
	from [Group] AS g 
	join [ChargeStation] AS cs on cs.GroupId = g.Id
	where cs.Id = {connector.ChargeStationId}
);

select c.*
from [Connector] AS c
join (
	select cs.Id
	from [ChargeStation] AS cs
	where cs.GroupId in (
		select g.Id 
		from [Group] AS g 
		join [ChargeStation] AS cs on cs.GroupId = g.Id
		where cs.Id = {connector.ChargeStationId}
	)
) AS cs on cs.Id = c.ChargeStationId;
";

			var result = await ComposeGroupFromMultipleQuery(sql);
			return result;
		}

		private async Task<Group> ComposeGroupFromMultipleQuery(string sql)
		{
			var multi = await sqlConnectionFactory.GetOpenConnection().QueryMultipleAsync(sql);
			var result = await multi.ReadSingleAsync<Group>();
			var chargeStations = (await multi.ReadAsync<ChargeStation>()).ToDictionary(cs => cs.Id, cs => cs);
			var connectors = await multi.ReadAsync<Connector>();

			result.ChargeStations.AddRange(chargeStations.Values);

			foreach (var c in connectors)
				chargeStations[c.ChargeStationId].Connectors.Add(c);

			return result;
		}
	}
}
