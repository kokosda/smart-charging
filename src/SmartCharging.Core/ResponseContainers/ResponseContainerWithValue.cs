using SmartCharging.Core.Interfaces;
using System;

namespace SmartCharging.Core.ResponseContainers
{
	public sealed class ResponseContainerWithValue<T> : ResponseContainer, IResponseContainerWithValue<T>
	{
		public T Value { get; init; }

		public ResponseContainerWithValue(ResponseContainer anotherResponseContainer)
		{
			if (anotherResponseContainer is null)
				throw new ArgumentNullException(nameof(anotherResponseContainer));

			IsSuccess = anotherResponseContainer.IsSuccess;
			AddMessage(anotherResponseContainer.Messages);
		}
	}
}
