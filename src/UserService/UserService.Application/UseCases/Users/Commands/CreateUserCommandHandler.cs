using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Extensions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Commands;

public class CreateUserCommandHandler(UserManager<User> userManager) : IRequestHandler<CreateUserCommand, Response>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.Username,
            Birthday = DateFormatExtension.ToDateTime(request.Birthday),
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception($"Ошибка при создании пользователя!\n{result}");

        await _userManager.AddToRoleAsync(user, "User");

        return new Response()
        {
            Token = "",
            StatusCode = 201,
            Message = "Success"
        };
    }
}