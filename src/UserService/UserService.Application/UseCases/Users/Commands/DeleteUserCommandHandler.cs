using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Commands;

public class DeleteUserCommandHandler(IUserDbContext dbContext, UserManager<User> userManager) : IRequestHandler<DeleteUserCommand, Response>
{
    private readonly IUserDbContext _dbContext = dbContext;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            throw new NotFoundException("Пользователь не найден!");

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Response()
        {
            Token = "",
            StatusCode = 200,
            Message = "Пользователь успешно удалён!"
        };
    }
}
