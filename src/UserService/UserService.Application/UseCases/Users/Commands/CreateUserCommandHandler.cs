using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Extensions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Commands;

public class CreateUserCommandHandler(UserManager<User> userManager) : IRequestHandler<CreateUserCommand, Response>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.Username) != null)
            throw new CustomException(400, "Пользователь с таким именем пользователя уже существует.");

        if (await _userManager.FindByEmailAsync(request.Email) != null)
            throw new CustomException(400, "Пользователь с таким адресом электронной почты уже существует.");

        var user = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.Username,
            Birthday = DateFormatExtension.ToDateTime(request.Birthday),
            Email = request.Email
        };

        var creationResult = await _userManager.CreateAsync(user, request.Password);

        if (!creationResult.Succeeded)
            throw new Exception($"Ошибка при создании пользователя!\n{creationResult}");

        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
            throw new Exception($"Ошибка при создании пользователя!\n{roleResult}");

        return new Response()
        {
            Token = "",
            StatusCode = 200,
            Message = "Пользователь успешно создан!"
        };
    }
}