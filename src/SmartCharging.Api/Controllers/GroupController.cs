using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.Groups;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class GroupController : ControllerBase
	{
		private readonly ICreateGroupHandler createGroupHandler;

		public GroupController(ICreateGroupHandler createGroupHandler)
		{
			this.createGroupHandler = createGroupHandler;
		}

		/// <summary>
		/// Updates max current in Amp.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(GroupDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateGroupRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await createGroupHandler.HandleWithValueAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(string.Empty, result.Messages);
				return BadRequest(ModelState);
			}

			return Created($"/{result.Value.Id}", result.Value);
		}
	}
}
