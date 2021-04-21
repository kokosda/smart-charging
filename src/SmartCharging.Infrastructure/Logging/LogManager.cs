using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Infrastructure.Logging
{
	public static class LogManager
	{
		private static ILoggerFactory loggerFactory = null;

		public static ILoggerFactory LoggerFactory
		{
			get
			{
				if (loggerFactory == null)
					loggerFactory = new LoggerFactory();

				return loggerFactory;
			}
		}

		public static ILog GetLogger(string name) => new Log(LoggerFactory.CreateLogger(name));

		public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
		{
			return app;
		}
	}
}
