using System;
using System.Collections.Generic;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Core.ResponseContainers
{
	public sealed class ResponseContainerWithValue<T> : ResponseContainer, IResponseContainerWithValue<T>
	{
		private bool isSuccess;

		public bool IsSuccess
		{
			get => isSuccess;
			init => isSuccess = value;
		}

		public T Value { get; init; }
		public string Messages => string.Join(Environment.NewLine, MessagesList);

		private List<string> MessagesList { get; init; }

		public ResponseContainerWithValue()
		{
			MessagesList = new List<string>();
			
		}

		public void AddMessage(string message)
		{
			MessagesList.Add(message);
		}

		public void AddErrorMessage(string message)
		{
			AddMessage(message);
			isSuccess = false;
		}
	}
}
