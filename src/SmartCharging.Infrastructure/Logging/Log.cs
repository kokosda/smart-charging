using System;
using Microsoft.Extensions.Logging;
using SmartCharging.Core.Interfaces;
using ILog4Net = log4net.ILog;

namespace SmartCharging.Infrastructure.Logging
{
	public sealed class Log : ILog
	{
		private readonly ILog4Net log;

		public Log(ILog4Net log)
		{
			this.log = log ?? throw new ArgumentNullException(nameof(log));
		}

		public void LogDebug(string message, Exception exception = null)
		{
			if (!log.IsDebugEnabled)
				return;

			log.Debug(message, exception);
		}

		public void LogError(string message, Exception exception = null)
		{
			if (!log.IsErrorEnabled)
				return;

			log.Error(message, exception);
		}

		public void LogInfo(string message, Exception exception = null)
		{
			if (!log.IsInfoEnabled)
				return;

			log.Info(message, exception);
		}

		public void LogWarn(string message, Exception exception = null)
		{
			if (!log.IsWarnEnabled)
				return;

			log.Warn(message, exception);
		}
	}
}
