using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Connectors;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ConnectorController : ControllerBase
	{
		private readonly IUpdateMaxCurrentConnectorHandler updateConnectorHandler;

		public ConnectorController(IUpdateMaxCurrentConnectorHandler updateConnectorHandler)
		{
			this.updateConnectorHandler = updateConnectorHandler;
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
				ModelState.AddModelError(string.Empty, result.Messages);
				return BadRequest(ModelState);
			}

			return NoContent();
		}
	}
}
