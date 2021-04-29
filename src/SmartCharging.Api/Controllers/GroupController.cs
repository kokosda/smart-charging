using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Application.Groups;
using SmartCharging.Domain.Groups;

namespace SmartCharging.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public sealed class GroupController : ControllerBase
	{
		private readonly ICreateGroupHandler createGroupHandler;
		private readonly IDeleteGroupHandler deleteGroupHandler;
		private readonly IGetIntIdEntityHandler<Group, GroupDto> getIntIdEntityHandler;

		public GroupController(ICreateGroupHandler createGroupHandler, IDeleteGroupHandler deleteGroupHandler, IGetIntIdEntityHandler<Group, GroupDto> getIntIdEntityHandler)
		{
			this.createGroupHandler = createGroupHandler;
			this.deleteGroupHandler = deleteGroupHandler;
			this.getIntIdEntityHandler = getIntIdEntityHandler;
		}

		[Route("{id}")]
		[HttpGet]
		[ProducesResponseType(typeof(GroupDto), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> Get([FromRoute] GetGroupRequest request)
		{
			var command = new GetIntIdEntityCommand<Group, GroupDto>()
			{
				DtoFactory = GroupDto.From,
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
		/// Creates a group.
		/// </summary>
		[Route("")]
		[HttpPost]
		[ProducesResponseType(typeof(GroupDto), (int)HttpStatusCode.Created)]
		public async Task<ActionResult> Create(CreateGroupRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await createGroupHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorsProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Created($"/{result.Value.Id}", result.Value);
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<ActionResult> Delete([FromRoute] DeleteGroupRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await deleteGroupHandler.HandleAsync(request);

			if (!result.IsSuccess)
			{
				ModelState.AddModelError(Constants.ModelState.ErrorsProperty, result.Messages);
				return BadRequest(ModelState);
			}

			return Ok();
		}
	}
}
