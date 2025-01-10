using MediatR;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Commands;

public class DeleteUserCommand : IRequest<Response>
{
    public Guid Id { get; set; }
}
