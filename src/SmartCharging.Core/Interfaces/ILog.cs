using System;

namespace SmartCharging.Core.Interfaces
{
	public interface ILog
	{
		void LogDebug(string message, Exception exception = null);
		void LogInfo(string message, Exception exception = null);
		void LogWarn(string message, Exception exception = null);
		void LogError(string message, Exception exception = null);
	}
}
