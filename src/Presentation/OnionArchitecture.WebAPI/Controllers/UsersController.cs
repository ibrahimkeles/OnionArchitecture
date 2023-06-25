using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.RegisterStudent;
using OnionArchitecture.WebAPI.Extensions;

namespace OnionArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("RegisterUser")]
        [Validation(typeof(RegisterUserValidator))]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
