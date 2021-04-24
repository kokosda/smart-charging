using System.Threading.Tasks;
using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Groups
{
	public sealed class CreateGroupHandler : GenericCommandHandlerBase<CreateGroupRequest, GroupDto>, ICreateGroupHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(CreateGroupHandler));
		private readonly IGroupRepository groupRepository;

		public CreateGroupHandler(IGroupRepository groupRepository)
		{
			this.groupRepository = groupRepository;
		}

		protected override async Task<IResponseContainerWithValue<GroupDto>> GetResultAsync(CreateGroupRequest request)
		{
			var result = new ResponseContainerWithValue<GroupDto>();
			var groupResponseContainer = Group.Create(request.Name, request.CapacityInAmps);

			if (!groupResponseContainer.IsSuccess)
			{
				result.JoinWith(groupResponseContainer);
				Log.Error(result.Messages);
			}
			else
			{
				var group = await groupRepository.CreateAsync(groupResponseContainer.Value);
				result = new ResponseContainerWithValue<GroupDto> { Value = GroupDto.From(group) };
				Log.Info($"Group {group.Name} with ID={group.Id} is created.");
			}

			return result;
		}
	}
}
