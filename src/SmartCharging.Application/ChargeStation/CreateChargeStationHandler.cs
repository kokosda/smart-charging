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
	public sealed class CreateChargeStationHandler : GenericCommandHandlerBase<CreateChargeStationRequest, ChargeStationDto>, ICreateChargeStationHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(CreateChargeStationHandler));
		private readonly IGroupRepository groupRepository;
		private readonly IGenericRepository<ChargeStation, int> chargeStationRepository;

		public CreateChargeStationHandler(IGroupRepository groupRepository, IGenericRepository<ChargeStation, int> chargeStationRepository)
		{
			this.groupRepository = groupRepository;
			this.chargeStationRepository = chargeStationRepository;
		}

		protected override async Task<IResponseContainerWithValue<ChargeStationDto>> GetResultAsync(CreateChargeStationRequest request)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			var result = new ResponseContainerWithValue<ChargeStationDto>();
			var group = await groupRepository.GetAsync(request.GroupId);

			if (group is null)
			{
				result.AddErrorMessage($"Group with ID={request.GroupId} is not found.");
				Log.Error(result.Messages);
				return result;
			}

			var chargeStationResponseContainer = ChargeStation.Create(group, request.Name);

			if (!chargeStationResponseContainer.IsSuccess)
			{
				result.JoinWith(chargeStationResponseContainer);
				Log.Error(result.Messages);
				return result;
			}

			var chargeStation = await chargeStationRepository.CreateAsync(chargeStationResponseContainer.Value);
			result = new ResponseContainerWithValue<ChargeStationDto> { Value = ChargeStationDto.From(chargeStation) };
			return result;
		}
	}
}
