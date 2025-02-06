using MediatR;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>
{
}
