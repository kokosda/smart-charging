using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Connectors;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ConnectorController : ControllerBase
	{
		private readonly IUpdateMaxCurrentConnectorHandler updateConnectorHandler;
		private readonly IGetIntIdEntityHandler<Connector, ConnectorDto> getIntIdEntityHandler;

		public ConnectorController(IUpdateMaxCurrentConnectorHandler updateConnectorHandler, IGetIntIdEntityHandler<Connector, ConnectorDto> getIntIdEntityHandler)
		{
			this.updateConnectorHandler = updateConnectorHandler;
			this.getIntIdEntityHandler = getIntIdEntityHandler;
		}

		[Route("{id}")]
		[HttpGet]
		[ProducesResponseType(typeof(ConnectorDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> Get([FromRoute] GetConnectorRequest request)
		{
			var command = new GetIntIdEntityCommand<Connector, ConnectorDto>()
			{
				DtoFactory = ConnectorDto.From,
				Request = request
			};

			var result = await getIntIdEntityHandler.HandleWithValueAsync(command);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Ok(result.Value);
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(ConnectorDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateGroupRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await createGroupHandler.HandleWithValueAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Created($"/{result.Value.Id}", result.Value);
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("UpdateMaxCurrent")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> UpdateMaxCurrent(UpdateConnectorRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await updateConnectorHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return NoContent();
		}
	}
}
