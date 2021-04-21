using System;
using System.Data;
using System.Data.SqlClient;
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
			if (connection == null || connection.State != ConnectionState.Open)
			{
				connection = new SqlConnection(connectionString);
				connection.Open();
			}

			return connection;
		}

		public void Dispose()
		{
			if (connection != null && connection.State == ConnectionState.Open)
			{
				connection.Dispose();
			}
		}
	}
}
