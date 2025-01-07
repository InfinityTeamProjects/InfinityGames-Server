using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities.Auth;

namespace UserService.Application.Abstractions;

public interface IUserDbContext
{
    DbSet<User> Users { get; set; }

    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
