using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Abstractions;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IUserDbContext, UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("LocalConnection")));

        return services;
    }
}
