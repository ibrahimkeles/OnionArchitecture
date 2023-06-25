using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.TodoItems.CreateTodoItem;
using OnionArchitecture.Application.Features.Commands.TodoItems.DeleteTodoItem;
using OnionArchitecture.Application.Features.Commands.TodoItems.UpdateTodoItem;
using OnionArchitecture.Application.Features.Queries.TodoItems.GetAllTodoItems;
using OnionArchitecture.WebAPI.Extensions;

namespace OnionArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllTodoItems")]
        public async Task<IActionResult> GetAllTodoItems([FromQuery] GetAllTodoItemsRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("CreateTodoItem")]
        [Validation(typeof(CreateTodoItemValidator))]
        public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut("UpdateTodoItem")]
        [Validation(typeof(UpdateTodoItemValidator))]
        public async Task<IActionResult> UpdateTodoItem([FromBody] UpdateTodoItemRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete("DeleteTodoItem")]
        public async Task<IActionResult> DeleteTodoItem([FromQuery] DeleteTodoItemRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
