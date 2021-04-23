namespace SmartCharging.Core.Interfaces
{
	public interface IResponseContainer
	{
		bool IsSuccess { get; }
		string Messages { get; }
	}
}
