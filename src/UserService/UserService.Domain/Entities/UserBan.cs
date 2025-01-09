using System.ComponentModel.DataAnnotations;
using UserService.Domain.Commons;
using UserService.Domain.Entities.Auth;

namespace UserService.Domain.Entities;

public class UserBan : Auditable
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    [MaxLength(20, ErrorMessage = "Причина бана превышает порог максимально допустимых символов!")]
    public string Reason { get; set; }
    public DateTime BannedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
}
