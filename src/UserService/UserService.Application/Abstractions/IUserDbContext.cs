namespace UserService.Application.Abstractions;

public interface IUserDbContext
{
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
