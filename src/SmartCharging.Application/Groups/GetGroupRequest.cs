using SmartCharging.Application.GeneralRequests;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Application.Groups
{
	public sealed class GetGroupRequest : GetIntIdEntityRequest<Group, GroupDto>
	{
	}
}
