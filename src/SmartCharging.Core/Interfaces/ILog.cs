using System;

namespace SmartCharging.Core.Interfaces
{
	public interface ILog
	{
		void Debug(string message, Exception exception = null);
		void Info(string message, Exception exception = null);
		void Warn(string message, Exception exception = null);
		void Error(string message, Exception exception = null);
	}
}
