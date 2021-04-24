using SmartCharging.Core.Handlers;

namespace SmartCharging.Application.Groups
{
	public interface ICreateGroupHandler : IGenericCommandHandler<CreateGroupRequest, GroupDto>
	{
	}
}
