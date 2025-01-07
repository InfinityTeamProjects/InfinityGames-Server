using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities.Auth;

public class User : IdentityUser<Guid>
{
    public string? ProfilePicture { get; set; }
    public decimal Wallet { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public virtual ICollection<Guid>? LibraryGameIds { get; set; }
    public virtual ICollection<Guid>? WishlistGameIds { get; set; }
    public virtual ICollection<Guid>? CartGameIds { get; set; }
}