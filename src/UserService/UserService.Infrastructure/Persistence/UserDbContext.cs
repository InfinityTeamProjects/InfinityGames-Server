using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;

namespace UserService.Infrastructure.Persistence;

public class UserDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IUserDbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {

    }

    async ValueTask<int> IUserDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        => await base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted && u.EmailConfirmed);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
