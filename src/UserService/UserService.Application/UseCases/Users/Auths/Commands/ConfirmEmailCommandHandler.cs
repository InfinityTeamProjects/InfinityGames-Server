using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class ConfirmEmailCommandHandler(ITokenService tokenService, IRedisService redisService, IUserDbContext dbContext, UserManager<User> userManager) : IRequestHandler<ConfirmEmailCommand, Response>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRedisService _redisService = redisService;
    private readonly IUserDbContext _dbContext = dbContext;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var verifyCode = await _redisService.GetAsync<VerifyCode>(request.Email);

        if (verifyCode == null)
            throw new CustomException(408, "Код подтверждения устарел!");
        else if (verifyCode.Code != request.Code)
            throw new CustomException(409, "Неверный код подтверждения!");

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            throw new NotFoundException("Пользователь не найден!");

        user.EmailConfirmed = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Response()
        {
            Token = _tokenService.GenerateTokenToUser(user),
            StatusCode = 200,
            Message = "Электронная почта подтверждена успешно!"
        };
    }
}
