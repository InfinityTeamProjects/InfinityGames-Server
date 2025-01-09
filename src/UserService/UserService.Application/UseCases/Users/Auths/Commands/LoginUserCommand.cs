using MediatR;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class LoginUserCommand : IRequest<Response>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
