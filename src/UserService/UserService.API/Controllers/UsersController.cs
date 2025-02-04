using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Users.Commands;
using UserService.Application.UseCases.Users.Queries;
using UserService.Domain.Entities.DTOs;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async ValueTask<ActionResult<Response>> CreateUserAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async ValueTask<ActionResult<Response>> UpdateUserAsync(UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async ValueTask<ActionResult<Response>> UpdateUserRoleAsync(UpdateUserRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async ValueTask<ActionResult<Response>> DeleteUserAsync(DeleteUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<UserDTO>>> GetAllUsersAsync(GetAllUsersQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<UserDTO>> GetUserByIdAsync(GetUserByIdQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
