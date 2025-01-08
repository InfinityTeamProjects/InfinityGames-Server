using MediatR;
using UserService.Domain.Entities.Auth;

namespace UserService.Application.UseCases.Users.Commands;

public class CreateUserCommand : IRequest<User>
{

}
