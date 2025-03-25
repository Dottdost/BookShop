namespace BookShop.Data.Models;

public class UserRole
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public string RoleName { get; set; } = string.Empty;
    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
    public User User { get; set; } = null!;
}