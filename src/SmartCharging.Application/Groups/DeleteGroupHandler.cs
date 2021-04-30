using System.Threading.Tasks;
using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Groups
{
	public sealed class DeleteGroupHandler : CommandHandlerBase<DeleteGroupRequest>, IDeleteGroupHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(DeleteGroupHandler));
		private readonly IGroupRepository groupRepository;

		public DeleteGroupHandler(IGroupRepository groupRepository)
		{
			this.groupRepository = groupRepository;
		}

		protected override async Task<IResponseContainer> GetResultAsync(DeleteGroupRequest request)
		{
			var result = new ResponseContainer();
			var group = await groupRepository.GetAsync(request.Id);

			if (group is not null)
			{
				await groupRepository.DeleteAsync(group.Id);
				Log.Info($"Group with ID={group.Id} has been deleted.");
			}
			else
			{
				var message = $"Group with ID={request.Id} is not found.";
				result.AddErrorMessage(message);
				Log.Error(message);
			}

			return result;
		}
	}
}
