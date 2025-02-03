using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using UserService.Application.Abstractions;

namespace UserService.Infrastructure.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private readonly int _expirationMinutes;

    public RedisService(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!);
        _database = redis.GetDatabase();
        _expirationMinutes = int.Parse(configuration["Redis:ExpirationMinutes"]!);
    }

    public async ValueTask<bool> AddAsync<T>(string key, T value)
    {
        return await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), TimeSpan.FromMinutes(_expirationMinutes));
    }

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);

        return JsonConvert.DeserializeObject<T>(value);
    }

    public async ValueTask<bool> RemoveAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}
