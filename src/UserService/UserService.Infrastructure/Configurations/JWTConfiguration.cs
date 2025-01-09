namespace UserService.Infrastructure.Configurations;

public class JWTConfiguration
{
    /// <summary>
    /// Допустимый получатель токена (Audience).
    /// Например, URL фронтенда или идентификатор клиента.
    /// </summary>
    public string ValidAudience { get; set; } = string.Empty;

    /// <summary>
    /// Издатель токена (Issuer).
    /// Например, URL сервера, который выдает токен.
    /// </summary>
    public string ValidIssuer { get; set; } = string.Empty;

    /// <summary>
    /// Секретный ключ для подписи токенов.
    /// Должен быть достаточно длинным и уникальным.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Срок жизни токена (в днях).
    /// </summary>
    public int ExpireDays { get; set; }
}