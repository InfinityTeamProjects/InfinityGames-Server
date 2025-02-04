using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Queries;

public class GetUserByIdQueryHandler(UserManager<User> userManager) : IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            return new UserDTO();

        return new UserDTO()
        {
            Name = user.Name,
            Surname = user.Surname,
            Username = user.UserName,
            Email = user.Email,
            Birthday = user.Birthday,
            ProfilePicture = user.ProfilePicture,
            Wallet = user.Wallet
        };
    }
}