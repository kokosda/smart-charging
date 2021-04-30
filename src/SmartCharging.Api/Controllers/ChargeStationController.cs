using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.ChargeStations;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Domain.ChargeStations;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ChargeStationController : ControllerBase
	{
		private readonly ICreateChargeStationHandler chargeStationHandler;
		private readonly IGetIntIdEntityHandler<ChargeStation, ChargeStationDto> getIntIdEntityHandler;
		private readonly IDeleteChargeStationHandler deleteChargeStationHandler;

		public ChargeStationController(ICreateChargeStationHandler chargeStationHandler, IGetIntIdEntityHandler<ChargeStation, ChargeStationDto> getIntIdEntityHandler, IDeleteChargeStationHandler deleteChargeStationHandler)
		{
			this.chargeStationHandler = chargeStationHandler;
			this.getIntIdEntityHandler = getIntIdEntityHandler;
			this.deleteChargeStationHandler = deleteChargeStationHandler;
		}

		[Route("{id}")]
		[HttpGet]
		[ProducesResponseType(typeof(ChargeStationDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> Get([FromRoute] GetChargeStationRequest request)
		{
			var command = new GetIntIdEntityCommand<ChargeStation, ChargeStationDto>()
			{
				DtoFactory = ChargeStationDto.From,
				Request = request
			};

			var result = await getIntIdEntityHandler.HandleAsync(command);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorsProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Ok(result.Value);
		}

		/// <summary>
		/// Creates charge station in a group.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(ChargeStationDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateChargeStationRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await chargeStationHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(string.Empty, result.Messages);
				return BadRequest(ModelState);
			}

			return Created($"/{result.Value.Id}", result.Value);
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<ActionResult> Delete([FromRoute] DeleteChargeStationRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await deleteChargeStationHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorsProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Ok();
		}
	}
}
