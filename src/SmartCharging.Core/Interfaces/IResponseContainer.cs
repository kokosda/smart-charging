namespace SmartCharging.Core.Interfaces
{
	public interface IResponseContainer
	{
		bool IsSuccess { get; }
		string Messages { get; }

		void AddMessage(string message);
		void AddErrorMessage(string message);
		IResponseContainer JoinWith(IResponseContainer anotherResponseContainer);
	}
}
