using MediatR;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Commands;

public class UpdateUserRoleCommand : IRequest<Response>
{
    public Guid Id { get; set; }
    public string Role { get; set; }
}
