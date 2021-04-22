using System;
using System.Threading.Tasks;
using Dapper;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Connectors;
using SmartCharging.Infrastructure.Database;

namespace SmartCharging.Infrastructure.Domain
{
	public sealed class ConnectorRepository : GenericRepository<Connector, int>, IConnectorRepository
	{
		private readonly ISqlConnectionFactory sqlConnectionFactory;

		public ConnectorRepository(ISqlConnectionFactory sqlConnectionFactory)
		{
			this.sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
		}
		public async Task<Connector> GetByChargeStationIdAndLineNo(int chargeStationId, int lineNo)
		{
			var sql = $"select * from Connector where chargeStationId={chargeStationId} and LineNo={lineNo};";
			var result = await sqlConnectionFactory.GetOpenConnection().QueryFirstOrDefaultAsync<Connector>(sql);
			return result;
		}
	}
}
