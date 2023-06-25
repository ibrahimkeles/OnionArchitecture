using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.TodoLists.CreateTodoList;
using OnionArchitecture.Application.Features.Commands.TodoLists.DeleteTodoList;
using OnionArchitecture.Application.Features.Commands.TodoLists.UpdateTodoList;
using OnionArchitecture.Application.Features.Queries.TodoLists.GetAllTodoLists;
using OnionArchitecture.WebAPI.Extensions;

namespace OnionArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TodoListsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateTodoList")]
        [Validation(typeof(CreateTodoListValidator))]
        public async Task<IActionResult> CreateTodoList([FromBody] CreateTodoListRequest request)
        {
            request.UserId = User.FindFirst("Id").Value;
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut("UpdateTodoList")]
        [Validation(typeof(UpdateTodoListValidator))]
        public async Task<IActionResult> UpdateTodoList([FromBody] UpdateTodoListRequest request)
        {
            request.UserId = User.FindFirst("Id").Value;
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete("DeleteTodoList")]
        public async Task<IActionResult> UpdateTodoList([FromQuery] DeleteTodoListRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("GetAllTodoLists")]
        public async Task<IActionResult> GetAllTodoLists()
        {
            var response = await _mediator.Send(new GetAllTodoListsRequest() { UserId = User.FindFirst("Id").Value });
            return Ok(response);
        }
    }
}
