using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities.Auth;
using UserService.Application.Abstractions;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistance;

public class UserDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IUserDbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
        => Database.Migrate();

    public DbSet<UserBan> UsersBans { get; set; }

    async ValueTask<int> IUserDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        => await base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}