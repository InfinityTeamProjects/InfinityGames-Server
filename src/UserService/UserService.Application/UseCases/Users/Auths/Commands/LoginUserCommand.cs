using MediatR;
using UserService.Domain.Entities.Auth;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class LoginUserCommand : IRequest<User>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
