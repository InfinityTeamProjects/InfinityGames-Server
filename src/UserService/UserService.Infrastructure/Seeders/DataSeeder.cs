using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
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
                    throw new Exception($"Ошибка создания роли '{roles[i]}'!\n{result.Errors.Select(e => e.Description)}");
            }
        }
    }

    private async Task SeedAdminAsync()
    {
        string name = _configuration["AdminSettings:Name"]!;
        string password = _configuration["AdminSettings:Password"]!;
        string email = _configuration["AdminSettings:Email"]!;
        string phoneNumber = _configuration["AdminSettings:PhoneNumber"]!;

        if (await _userManager.FindByNameAsync(name) == null)
        {
            var admin = new User()
            {
                Name = name,
                UserName = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Wallet = 100000000000,
                Birthday = new DateTime(2005, 8, 14),
                ProfilePicture = "https://ih1.redbubble.net/image.2955130987.9629/raf,360x360,075,t,fafafa:ca443f4786.jpg",
            };

            var result = await _userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
            else
                throw new Exception($"Ошибка создания администратора!\n{result.Errors.Select(e => e.Description)}");
        }
    }
}
