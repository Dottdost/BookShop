namespace BookShop.Data.Models;

public class Role
{
    public string RoleName { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}