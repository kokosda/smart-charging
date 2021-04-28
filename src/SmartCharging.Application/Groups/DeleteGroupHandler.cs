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
			await groupRepository.DeleteAsync(request.Id);

			Log.Info($"Group with ID={request.Id} has been deleted.");

			return new ResponseContainer();
		}
	}
}
