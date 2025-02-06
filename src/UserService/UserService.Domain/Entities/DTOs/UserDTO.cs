namespace UserService.Domain.Entities.DTOs;

public class UserDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime Birthday { get; set; }
    public string? ProfilePicture { get; set; }
    public decimal Wallet { get; set; }
}
