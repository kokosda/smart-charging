using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCharging.Core.Interfaces;

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
