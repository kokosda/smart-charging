using SmartCharging.Application.Handlers;
using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Infrastructure.Logging;
using System.Threading.Tasks;

namespace SmartCharging.Application.ChargeStations
{
	public sealed class DeleteChargeStationHandler : CommandHandlerBase<DeleteChargeStationRequest>, IDeleteChargeStationHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(DeleteChargeStationHandler));
		private readonly IGenericRepository<ChargeStation, int> chargeStationRepository;

		public DeleteChargeStationHandler(IGenericRepository<ChargeStation, int> chargeStationRepository)
		{
			this.chargeStationRepository = chargeStationRepository;
		}

		protected override async Task<IResponseContainer> GetResultAsync(DeleteChargeStationRequest request)
		{
			var result = new ResponseContainer();
			var chargeStation = await chargeStationRepository.GetAsync(request.Id);

			if (chargeStation is not null)
			{
				await chargeStationRepository.DeleteAsync(chargeStation.Id);
				Log.Info($"Charge station ID={chargeStation.Id} has been deleted.");
			}
			else
			{
				var message = $"Charge station with ID={request.Id} is not found.";
				result.AddErrorMessage(message);
				Log.Error(message);
			}

			return result;
		}
	}
}
