namespace SmartCharging.Application.Responses
{
	public abstract record ResponseBase
	{
		string Message { get; init; }
	}
}
