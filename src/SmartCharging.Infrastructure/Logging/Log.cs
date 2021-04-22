using System;
using Microsoft.Extensions.Logging;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Infrastructure.Logging
{
	public sealed class Log : ILog
	{
		private readonly ILogger logger;

		public Log(ILogger logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void LogDebug(string message, Exception exception = null)
		{
			if (!logger.IsEnabled(LogLevel.Debug))
				return;

			if (exception is null)
				logger.LogDebug(message);
			else
				logger.LogDebug(exception, message);
		}

		public void LogError(string message, Exception exception = null)
		{
			if (!logger.IsEnabled(LogLevel.Error))
				return;

			if (exception is null)
				logger.LogError(message);
			else
				logger.LogError(exception, message);
		}

		public void LogInfo(string message, Exception exception = null)
		{
			if (!logger.IsEnabled(LogLevel.Information))
				return;

			if (exception is null)
				logger.LogInformation(message);
			else
				logger.LogInformation(exception, message);
		}

		public void LogWarn(string message, Exception exception = null)
		{
			if (!logger.IsEnabled(LogLevel.Warning))
				return;

			if (exception is null)
				logger.LogWarning(message);
			else
				logger.LogWarning(exception, message);
		}
	}
}
