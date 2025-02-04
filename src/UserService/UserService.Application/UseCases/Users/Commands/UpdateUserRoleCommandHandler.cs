using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Commands;

public class UpdateUserRoleCommandHandler(IUserDbContext dbContext, UserManager<User> userManager) : IRequestHandler<UpdateUserRoleCommand, Response>
{
    private readonly IUserDbContext _dbContext = dbContext;
    private readonly UserManager<User> _userManager = userManager;
    private readonly string userRole = "User";
    private readonly string adminRole = "Admin";

    public async Task<Response> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            throw new NotFoundException("Пользователь не найден!");

        switch (request.Role)
        {
            case "Admin":
                await _userManager.RemoveFromRoleAsync(user, userRole);

                await _userManager.AddToRoleAsync(user, adminRole);

                break;

            case "User":
                await _userManager.RemoveFromRoleAsync(user, adminRole);

                await _userManager.AddToRoleAsync(user, userRole);

                break;

            default:
                throw new CustomException(404, "Такой роли не существует!");
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Response()
        {
            Token = "",
            StatusCode = 200,
            Message = "Роль пользователя успешно изменена!"
        };
    }
}