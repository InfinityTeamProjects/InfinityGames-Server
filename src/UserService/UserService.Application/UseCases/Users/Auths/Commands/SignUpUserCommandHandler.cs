using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Application.UseCases.Users.Commands;
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

        await CreateUserAsync(request, cancellationToken);

        var user = await _userManager.FindByNameAsync(request.Username);

        string token = _tokenService.GenerateTokenToUser(user);

        return new Response()
        {
            Token = token,
            StatusCode = 201,
            Message = "Success"
        };
    }

    private async ValueTask CreateUserAsync(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        var createUserCommand = new CreateUserCommand()
        {
            Name = request.Name,
            Surname = request.Surname,
            Username = request.Username,
            Birthday = request.Birthday,
            Email = request.Email,
            Password = request.Password
        };

        var createUserCommandHandler = new CreateUserCommandHandler(_userManager);
        await createUserCommandHandler.Handle(createUserCommand, cancellationToken);
    }
}