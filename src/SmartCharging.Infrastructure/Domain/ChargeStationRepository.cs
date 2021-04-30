using Dapper;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Database;
using System.Threading.Tasks;

namespace SmartCharging.Infrastructure.Domain
{
	public sealed class ChargeStationRepository : GenericRepository<ChargeStation, int>
	{
		public ChargeStationRepository(ISqlConnectionFactory factory) : base(factory)
		{
		}

		public override async Task<ChargeStation> GetAsync(int id)
		{
			var sql = $@"
select *
from [ChargeStation]
where Id = {id};

select *
from [Connector] c
join [ChargeStation] cs on cs.Id = c.ChargeStationId
where cs.Id = {id};
";
			var query = await sqlConnectionFactory.GetOpenConnection().QueryMultipleAsync(sql);
			var result = await query.ReadSingleOrDefaultAsync<ChargeStation>();

			if (result is not null)
			{
				var connectors = await query.ReadAsync<Connector>();
				result.Connectors.AddRange(connectors);
			}

			return result;
		}

		public override async Task DeleteAsync(int id)
		{
			var sql = $@"
delete from [Connector]
where Id in (
	select c.Id
	from [Connector] c
	inner join [ChargeStation] cs on cs.Id = {id}
);

delete from [ChargeStation]
where Id = {id};
";

			await sqlConnectionFactory.GetOpenConnection().ExecuteAsync(sql);
		}
	}
}
