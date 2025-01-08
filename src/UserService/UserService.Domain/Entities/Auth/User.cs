using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities.Auth;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public DateTime Birthday { get; set; }
    public string? ProfilePicture { get; set; }
    public decimal Wallet { get; set; }
    public ICollection<Guid>? LibraryGameIds { get; set; } = new List<Guid>();
    public ICollection<Guid>? WishlistGameIds { get; set; } = new List<Guid>();
    public ICollection<Guid>? CartGameIds { get; set; } = new List<Guid>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}