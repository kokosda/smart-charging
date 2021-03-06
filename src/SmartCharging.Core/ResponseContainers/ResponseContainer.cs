using SmartCharging.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace SmartCharging.Core.ResponseContainers
{
	public class ResponseContainer : IResponseContainer
	{
		private bool isSuccess;

		public bool IsSuccess
		{
			get => isSuccess;
			init => isSuccess = value;
		}

		public string Messages => string.Join(Environment.NewLine, MessagesList);

		private List<string> MessagesList { get; init; }

		public ResponseContainer()
		{
			MessagesList = new List<string>();
			IsSuccess = true;
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

		public IResponseContainer JoinWith(IResponseContainer anotherResponseContainer)
		{
			if (anotherResponseContainer is null)
				anotherResponseContainer = new ResponseContainer();

			isSuccess = anotherResponseContainer.IsSuccess;

			if (!string.IsNullOrWhiteSpace(anotherResponseContainer.Messages))
				MessagesList.Add(anotherResponseContainer.Messages);

			return this;
		}

		public IResponseContainer AsInterface()
		{
			return this;
		}
	}
}
