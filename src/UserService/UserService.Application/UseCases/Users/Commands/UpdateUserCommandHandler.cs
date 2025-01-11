using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Users.Commands;

public class UpdateUserCommandHandler(IUserDbContext dbContext, IHostEnvironment hostEnvironment, UserManager<User> userManager) : IRequestHandler<UpdateUserCommand, Response>
{
    private readonly IUserDbContext _dbContext = dbContext;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Response> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            throw new NotFoundException("Пользователь не найден!");

        else if (await _userManager.FindByNameAsync(request.Username) != null)
            throw new CustomException(400, "Пользователь с таким именем пользователя уже существует.");

        else if (await _userManager.FindByEmailAsync(request.Email) != null)
            throw new CustomException(400, "Пользователь с таким адресом электронной почты уже существует.");

        string pictureName = "";
        string picturePath = "";

        try
        {
            pictureName = Guid.NewGuid().ToString() + Path.GetExtension(request.ProfilePicture.FileName);
            picturePath = Path.Combine(_hostEnvironment.ContentRootPath, $"{request.Username}", pictureName);

            using (var pictureStream = new FileStream(picturePath, FileMode.Create))
            {
                await request.ProfilePicture.CopyToAsync(pictureStream);
            }
        }
        catch
        {
            throw new Exception("Ошибка при загрузке изображения профиля!");
        }

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.UserName = request.Username;
        user.ProfilePicture = $"/{request.Username}/{pictureName}";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Response()
        {
            Token = "",
            StatusCode = 200,
            Message = "Пользователь успешно изменён!"
        };
    }
}