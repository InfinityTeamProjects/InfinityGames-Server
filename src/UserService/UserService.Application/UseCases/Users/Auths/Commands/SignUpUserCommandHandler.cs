using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions;
using UserService.Application.Extensions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class SignUpUserCommandHandler(UserManager<User> userManager, IUserDbContext dbContext, IRedisService redisService, IEmailService emailService) : IRequestHandler<SignUpUserCommand, Response>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUserDbContext _dbContext = dbContext;
    private readonly IRedisService _redisService = redisService;
    private readonly IEmailService _emailService = emailService;

    public async Task<Response> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.Username) != null)
            throw new CustomException(400, "Пользователь с таким именем уже существует.");

        var user = await _userManager.Users.IgnoreQueryFilters()
                                                .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user != null && !user.IsDeleted && user.EmailConfirmed)
            throw new CustomException(400, "Пользователь с таким адресом электронной почты уже существует.");

        else if (user != null)
        {
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.UserName = request.Username;
            user.Birthday = DateFormatExtension.ToDateTime(request.Birthday);

            user.IsDeleted = false;
            user.DeletedAt = null;

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);

            if (!passwordChangeResult.Succeeded)
                throw new Exception($"Ошибка при создании пользователя! {passwordChangeResult}");
        }
        else
        {
            user = new User()
            {
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.Username,
                Birthday = DateFormatExtension.ToDateTime(request.Birthday),
                Email = request.Email,
                EmailConfirmed = false,
            };

            var creationResult = await _userManager.CreateAsync(user, request.Password);

            if (!creationResult.Succeeded)
                throw new Exception($"Ошибка при создании пользователя! {creationResult}");

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
                throw new Exception($"Ошибка при создании пользователя! {roleResult}");
        }

        var confirmationCode = new Random().Next(100000, 999999).ToString();

        var emailConfirmation = new VerifyCode()
        {
            Email = request.Email,
            Code = confirmationCode,
        };

        await _redisService.AddAsync(emailConfirmation.Email, emailConfirmation.Code);

        await _emailService.SendEmailAsync(request.Email, "Email Confirmation Code", $"Your confirmation code is: {confirmationCode}");

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Response()
        {
            Token = "",
            StatusCode = 201,
            Message = "Подтвердите электронную почту."
        };
    }
}
