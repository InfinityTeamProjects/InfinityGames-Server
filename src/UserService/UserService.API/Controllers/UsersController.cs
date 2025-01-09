using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Users.Auths.Commands;
using UserService.Domain.Entities.DTOs;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("SignUp")]
        public async Task<Response> SignUpUserAsync(SignUpUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("Login")]
        public async Task<Response> LoginUserAsync(LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        //[HttpGet("Check")]
        //public IActionResult Check()
        //{
        //    return Ok("Works!");
        //}
    }
}
