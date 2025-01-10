using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Users.Commands;
using UserService.Domain.Entities.DTOs;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async ValueTask<Response> CreateUserAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut("{id}")]
        public async ValueTask<Response> UpdateUserAsync(UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}")]
        public async ValueTask<Response> DeleteUserAsync(DeleteUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
