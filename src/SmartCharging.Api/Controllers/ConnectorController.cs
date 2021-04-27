﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Connectors;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Application.Groups;
using SmartCharging.Domain.Connectors;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ConnectorController : ControllerBase
	{
		private readonly IUpdateMaxCurrentConnectorHandler updateConnectorHandler;
		private readonly IGetIntIdEntityHandler<Connector, ConnectorDto> getIntIdEntityHandler;
		private readonly ICreateConnectorHandler createConnectorHandler;

		public ConnectorController(IUpdateMaxCurrentConnectorHandler updateConnectorHandler, IGetIntIdEntityHandler<Connector, ConnectorDto> getIntIdEntityHandler, ICreateConnectorHandler createConnectorHandler)
		{
			this.updateConnectorHandler = updateConnectorHandler;
			this.getIntIdEntityHandler = getIntIdEntityHandler;
			this.createConnectorHandler = createConnectorHandler;
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

			var result = await getIntIdEntityHandler.HandleAsync(command);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Ok(result.Value);
		}

		/// <summary>
		/// Creates connector or returns a list of suggestion of what connectors have to removed from the group to
		/// free the exact amount of capacity for the connector adding.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(ConnectorDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateConnectorRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await createConnectorHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorProperty, result.Messages);
				return BadRequest(ModelState);
			}

			if (result.Value.RemovalSuggestions.Any())
				return Ok(result.Value.RemovalSuggestions);

			return Created($"/", result.Value.Connector);
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
