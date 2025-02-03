namespace UserService.Application.Abstractions;

public interface IRedisService
{
    ValueTask<bool> AddAsync<T>(string key, T value);
    ValueTask<T?> GetAsync<T>(string key);
    ValueTask<bool> RemoveAsync(string key);
}
