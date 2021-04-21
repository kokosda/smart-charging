using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartCharging.Application.Connectors;
using SmartCharging.Core.Interfaces;
using SmartCharging.Infrastructure.Logging;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class ConnectorController : ControllerBase
	{
		private static readonly ILog Log = LogManager.GetLogger(nameof(ConnectorController));
		private readonly IUpdateConnectorHandler updateConnectorHandler;

		public ConnectorController(IUpdateConnectorHandler updateConnectorHandler)
		{
			this.updateConnectorHandler = updateConnectorHandler ?? throw new ArgumentNullException(nameof(updateConnectorHandler));
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult> UpdateMaxCurrent([FromBody] UpdateConnectorRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await updateConnectorHandler.UpdateMaxCurrentAsync(request);

			return NoContent();
		}
	}
}
