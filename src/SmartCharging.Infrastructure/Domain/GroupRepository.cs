using Dapper;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Database;
using System.Threading.Tasks;

namespace SmartCharging.Infrastructure.Domain
{
	public sealed class GroupRepository : GenericRepository<Group, int>, IGroupRepository
	{
		private readonly ISqlConnectionFactory sqlConnectionFactory;

		public GroupRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
		{
			this.sqlConnectionFactory = sqlConnectionFactory;
		}

		public async Task<Group> GetByConnector(Connector connector)
		{
			var sql = @$"
select g.* 
from [Group] AS g
join [ChargeStation] AS cs on cs.GroupId = g.Id
join [Connector] AS c on c.ChargeStationId = cs.Id
where c.{nameof(connector.ChargeStationId)}={connector.ChargeStationId} and c.{nameof(connector.LineNo)}={connector.LineNo};
";

			var result = await sqlConnectionFactory.GetOpenConnection().QueryFirstOrDefaultAsync<Group>(sql);
			return result;
		}
	}
}
