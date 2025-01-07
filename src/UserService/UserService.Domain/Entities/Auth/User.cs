using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities.Auth;

public class User : IdentityUser<Guid>
{
    public string? Picture { get; set; }
    public decimal Wallet { get; set; }
    public virtual ICollection<Guid>? LibraryGameIds { get; set; }
    public virtual ICollection<Guid>? WishlistGameIds { get; set; }
    public virtual ICollection<Guid>? CartGameIds { get; set; }
}