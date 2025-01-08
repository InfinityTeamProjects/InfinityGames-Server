using UserService.Domain.Entities.Auth;

namespace UserService.Application.Abstractions;

public interface ITokenService
{
    public string GenerateTokenToAdmin(User user);
    public string GenerateTokenToUser(User user);
}
