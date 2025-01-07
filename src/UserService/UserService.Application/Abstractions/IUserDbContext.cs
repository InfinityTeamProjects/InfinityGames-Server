using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Application.Abstractions;

public interface IUserDbContext
{
    DbSet<UserBan> UsersBans { get; set; }

    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
