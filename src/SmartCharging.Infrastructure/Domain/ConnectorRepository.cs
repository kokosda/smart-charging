using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Database;

namespace SmartCharging.Infrastructure.Domain
{
	public sealed class ConnectorRepository : GenericRepository<Connector, int>, IConnectorRepository
	{
		public ConnectorRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
		{
		}

		public async Task<Connector> GetByChargeStationIdAndLineNoAsync(int chargeStationId, int lineNo)
		{
			var sql = $"select * from Connector where chargeStationId={chargeStationId} and LineNo={lineNo};";
			var result = await sqlConnectionFactory.GetOpenConnection().QueryFirstOrDefaultAsync<Connector>(sql);
			return result;
		}

		public async Task<IReadOnlyList<Connector>> GetAllInGroupByChargeStationIdAsync(int chargeStationId)
		{
			var sql = $@"
select c.*
from [Connector] AS c
join [ChargeStation] AS cs on cs.Id = c.ChargeStationId
join (
	select cs.GroupId
	from [ChargeStation] cs
	where cs.Id = {chargeStationId}
) t on t.GroupId = cs.GroupId
order by c.{nameof(Connector.MaxCurrentInAmps)} desc;
";
			var result = await sqlConnectionFactory.GetOpenConnection().QueryAsync<Connector>(sql);
			return result.ToList();
		}
	}
}
