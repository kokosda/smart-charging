﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Connectors;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ChargeStationController : ControllerBase
	{
		private readonly ICreateChargeStationHandler chargeStationHandler;

		public ChargeStationController(ICreateChargeStationHandler chargeStationHandler)
		{
			this.chargeStationHandler = chargeStationHandler;
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(ChargeStationDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateChargeStationRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await chargeStationHandler.HandleWithValueAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(string.Empty, result.Messages);
				return BadRequest(ModelState);
			}

			return Created($"/{result.Value.Id}", result.Value);
		}
	}
}