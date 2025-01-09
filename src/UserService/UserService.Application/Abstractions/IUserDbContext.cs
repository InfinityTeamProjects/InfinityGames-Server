using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions;

public interface IUserDbContext
{
    DbSet<UserBan> UserBans { get; set; }

    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
