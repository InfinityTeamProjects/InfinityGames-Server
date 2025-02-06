using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Queries;

public class GetAllUsersQueryHandler(UserManager<User> userManager) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.Select(u => new UserDTO()
                                                               {
                                                                   Name = u.Name,
                                                                   Surname = u.Surname,
                                                                   Username = u.UserName,
                                                                   Email = u.Email,
                                                                   Birthday = u.Birthday,
                                                                   ProfilePicture = u.ProfilePicture,
                                                                   Wallet = u.Wallet,
                                                               }).ToListAsync(cancellationToken);

        return users.AsEnumerable();
    }
}
