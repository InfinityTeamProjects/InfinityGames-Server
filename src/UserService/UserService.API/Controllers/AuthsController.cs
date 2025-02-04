using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Users.Auths.Commands;
using UserService.Domain.Entities.DTOs;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Login")]
        public async ValueTask<ActionResult<Response>> LoginUserAsync(LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("SignUp")]
        public async ValueTask<ActionResult<Response>> SignUpUserAsync(SignUpUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("EmailConfirmation")]
        public async ValueTask<ActionResult<Response>> ConfirmEmailAsync(ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
