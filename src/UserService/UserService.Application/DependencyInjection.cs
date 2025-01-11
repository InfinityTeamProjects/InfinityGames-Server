using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserService.Application.Abstractions;
using UserService.Application.Services;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
