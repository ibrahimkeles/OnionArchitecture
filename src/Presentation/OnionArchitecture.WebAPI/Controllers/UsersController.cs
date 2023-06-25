using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.User.LoginUser;
using OnionArchitecture.Application.Features.Commands.User.RegisterUser;
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
        [HttpPost("LoginUser")]
        [Validation(typeof(LoginUserValidator))]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
