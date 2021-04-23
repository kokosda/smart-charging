namespace SmartCharging.Core.Interfaces
{
	public interface IResponseContainerWithValue<T> : IResponseContainer
	{
		T Value { get; }
	}
}
