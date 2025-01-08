using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class SignUpUserCommandHandler(ITokenService tokenService, UserManager<User> userManager) : IRequestHandler<SignUpUserCommand, Response>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
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
            Birthday = DateTime.Parse(request.Birthday, CultureInfo.InvariantCulture),
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception("Ошибка при создании пользователя!");

        await _userManager.AddToRoleAsync(user, "User");
        
        string token = _tokenService.GenerateTokenToUser(user);

        return new Response()
        {
            Token = token,
            StatusCode = 201,
            Message = "Success"
        };
    }
}