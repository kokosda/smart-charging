using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Connectors;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ChargeStationController : ControllerBase
	{
		private readonly IUpdateConnectorHandler updateConnectorHandler;

		public ChargeStationController(IUpdateConnectorHandler updateConnectorHandler)
		{
			this.updateConnectorHandler = updateConnectorHandler ?? throw new ArgumentNullException(nameof(updateConnectorHandler));
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> Create(UpdateConnectorRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await updateConnectorHandler.UpdateMaxCurrentAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(string.Empty, result.Messages);
				return BadRequest(ModelState);
			}

			return NoContent();
		}
	}
}
