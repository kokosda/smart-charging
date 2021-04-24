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

		public void Debug(string message, Exception exception = null)
		{
			if (!log.IsDebugEnabled)
				return;

			log.Debug(message, exception);
		}

		public void Error(string message, Exception exception = null)
		{
			if (!log.IsErrorEnabled)
				return;

			log.Error(message, exception);
		}

		public void Info(string message, Exception exception = null)
		{
			if (!log.IsInfoEnabled)
				return;

			log.Info(message, exception);
		}

		public void Warn(string message, Exception exception = null)
		{
			if (!log.IsWarnEnabled)
				return;

			log.Warn(message, exception);
		}
	}
}
