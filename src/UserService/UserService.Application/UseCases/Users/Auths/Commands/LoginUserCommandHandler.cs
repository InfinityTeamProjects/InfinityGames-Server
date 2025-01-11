using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class LoginUserCommandHandler(ITokenService tokenService, UserManager<User> userManager) : IRequestHandler<LoginUserCommand, Response>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new CustomException(400, "Адрес электронной почты и пароль не совпадают.");

        var checker = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!checker)
            throw new CustomException(400, "Адрес электронной почты и пароль не совпадают.");

        string token = null;
        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            token = _tokenService.GenerateTokenToAdmin(user);
        }
        else
            token = _tokenService.GenerateTokenToUser(user);

        return new Response()
        {
            Token = token,
            StatusCode = 200,
            Message = "Успешный вход!"
        };
    }
}
