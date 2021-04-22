using System;
using System.Data;
using System.Data.SQLite;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Infrastructure.Database
{
	public sealed class SqlConnectionFactory : ISqlConnectionFactory
	{
		private readonly string connectionString;
		private IDbConnection connection;

		public SqlConnectionFactory(string connectionString)
		{
			this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
		}

		public IDbConnection GetOpenConnection()
		{
			if (connection is not { State: ConnectionState.Open })
			{ ;
				connection = new SQLiteConnection(connectionString);
				connection.Open();
			}

			return connection;
		}

		public void Dispose()
		{
			if (connection is { State: ConnectionState.Open })
			{
				connection.Dispose();
			}
		}
	}
}
