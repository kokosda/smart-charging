using System.Data;

namespace SmartCharging.Core.Interfaces
{
	public interface ISqlConnectionFactory
	{
		IDbConnection GetOpenConnection();
	}
}
