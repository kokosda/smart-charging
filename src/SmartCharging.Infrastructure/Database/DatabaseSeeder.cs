using System;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmartCharging.Core.Interfaces;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Infrastructure.Database
{
	public static class DatabaseSeeder
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(DatabaseSeeder));

		public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
		{
			var factory = app.ApplicationServices.GetService<ISqlConnectionFactory>() ?? throw new NullReferenceException("SQL connection factory is not registered.");

			var connection = factory.GetOpenConnection();
			Seed(connection);
			Log.Info("Database seeded.");
			return app;
		}

		public static void Seed(IDbConnection connection)
		{
			CreateSchema(connection);
			InsertData(connection);
		}

		private static void CreateSchema(IDbConnection connection)
		{
			var sql = @"
CREATE TABLE [Group] (
    [Id]      INTEGER PRIMARY KEY             AUTOINCREMENT NOT NULL,
    [Name]           NVARCHAR (127)  NOT NULL,
    [CapacityInAmps] DECIMAL (10, 6) NOT NULL
);

CREATE TABLE [ChargeStation] (
    [Id]      INTEGER PRIMARY KEY            AUTOINCREMENT NOT NULL,
    [Name]    NVARCHAR (127) NOT NULL,
    [GroupId] INT            NOT NULL
);

CREATE TABLE [Connector] (
    [Id]      INTEGER PRIMARY KEY            AUTOINCREMENT NOT NULL,
    [ChargeStationId]  INT             NOT NULL,
    [MaxCurrentInAmps] DECIMAL (10, 6) NOT NULL,
    [LineNo]           INT             NOT NULL
);

CREATE UNIQUE INDEX [UI_Connector_ChargeStationId_LineNo]
    ON [Connector]([ChargeStationId] ASC, [LineNo] ASC);
";
			
			ExecuteSql(sql, connection);
		}

		private static void InsertData(IDbConnection connection)
		{
			var sql = @"
INSERT INTO [Group] ([Name], [CapacityInAmps])
VALUES ('Group1', 150);

INSERT INTO [Group] ([Name], [CapacityInAmps])
VALUES ('Group2', 120);

INSERT INTO [ChargeStation] ([Name], [GroupId])
VALUES ('Station 1', 1);

INSERT INTO [ChargeStation] ([Name], [GroupId])
VALUES ('Station 2', 1);

INSERT INTO [ChargeStation] ([Name], [GroupId])
VALUES ('Station 3', 2);

INSERT INTO [Connector] ([ChargeStationId], [MaxCurrentInAmps], [LineNo])
VALUES (1, 21, 1);

INSERT INTO [Connector] ([ChargeStationId], [MaxCurrentInAmps], [LineNo])
VALUES (1, 42, 2);

INSERT INTO [Connector] ([ChargeStationId], [MaxCurrentInAmps], [LineNo])
VALUES (1, 17, 3);

INSERT INTO [Connector] ([ChargeStationId], [MaxCurrentInAmps], [LineNo])
VALUES (2, 13, 1);

INSERT INTO [Connector] ([ChargeStationId], [MaxCurrentInAmps], [LineNo])
VALUES (2, 27, 2);
";

			ExecuteSql(sql, connection);
		}

		private static void ExecuteSql(string sql, IDbConnection connection)
		{
			var command = connection.CreateCommand();
			command.CommandText = sql;
			command.ExecuteNonQuery();
		}
	}
}
