using System;
using System.Threading.Tasks;
using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Application.Connectors
{
	public sealed class CreateChargeStationHandler : CommandHandlerBase<CreateChargeStationRequest>, ICreateChargeStationHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(CreateChargeStationHandler));
		private readonly IGroupRepository groupRepository;

		public CreateChargeStationHandler(IGroupRepository groupRepository)
		{
			this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
		}

		protected override async Task<IResponseContainer> GetResultAsync(CreateChargeStationRequest request)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var result = new ResponseContainer();
			var group = await groupRepository.GetAsync(request.GroupId);

			if (group is null)
			{
				result.AddErrorMessage($"Group with ID={request.GroupId} is not found.");
				Log.LogError(result.Messages);
				return result;
			}

			ChargeStation.
		}
	}
}
