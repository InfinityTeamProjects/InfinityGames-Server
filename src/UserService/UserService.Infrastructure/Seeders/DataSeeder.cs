using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using UserService.Application.Extensions;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Exceptions;

namespace UserService.Infrastructure.Seeders;

public class DataSeeder(IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly UserManager<User> _userManager = userManager;

    public async Task SeedDataAsync()
    {
        await SeedRolesAsync();
        await SeedAdminAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = _configuration.GetSection("Roles").Get<string[]>();

        if (roles == null || roles.Length == 0)
        {
            throw new NotFoundException("Роли отсутствуют в конфигурации!");
        }

        for (short i = 0; i < roles!.Length; i++)
        {
            if (!await _roleManager.RoleExistsAsync(roles[i]))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roles[i]));

                if (!result.Succeeded)
                    throw new Exception($"Ошибка при создании роли '{roles[i]}'!\n{result}");
            }
        }
    }

    private async Task SeedAdminAsync()
    {
        string name = _configuration["AdminSettings:Name"]!;
        string surname = _configuration["AdminSettings:Surname"]!;
        string username = _configuration["AdminSettings:UserName"]!;
        string email = _configuration["AdminSettings:Email"]!;

        if (await _userManager.FindByNameAsync(username) == null)
        {
            var admin = new User()
            {
                Name = name,
                Surname = surname,
                UserName = username,
                Email = email,
                Wallet = 100000000000,
                Birthday = DateFormatExtension.ToDateTime("2005/08/14"),
                ProfilePicture = "https://ih1.redbubble.net/image.2955130987.9629/raf,360x360,075,t,fafafa:ca443f4786.jpg",
            };

            var result = await _userManager.CreateAsync(admin, username);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
            else
                throw new Exception($"Ошибка при создании администратора!\n{result}");
        }
    }
}
