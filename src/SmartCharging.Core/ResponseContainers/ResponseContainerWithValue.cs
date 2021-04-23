using SmartCharging.Core.Interfaces;

namespace SmartCharging.Core.ResponseContainers
{
	public sealed class ResponseContainerWithValue<T> : ResponseContainer, IResponseContainerWithValue<T>
	{
		public T Value { get; init; }
	}
}
