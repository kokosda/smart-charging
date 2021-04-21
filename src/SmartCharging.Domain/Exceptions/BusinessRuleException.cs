using System;

namespace SmartCharging.Domain.Exceptions
{
	public sealed class BusinessRuleException : Exception
	{
		public BusinessRuleException(string message) : base(message)
		{
		}
	}
}
