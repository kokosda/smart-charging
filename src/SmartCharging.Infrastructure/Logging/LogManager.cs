using System.IO;
using System.Reflection;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using SmartCharging.Core.Interfaces;
using LogManagerLog4Net = log4net.LogManager;

namespace SmartCharging.Infrastructure.Logging
{
	public static class LogManager
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(LogManager));

		public static ILog GetLogger(string name) => new Log(LogManagerLog4Net.GetLogger(name));

		public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
		{
			var logRepository = LogManagerLog4Net.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
			Log.Info("Logging configured.");
			return app;
		}
	}
}
