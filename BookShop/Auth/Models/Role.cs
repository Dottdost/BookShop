namespace BookShop.Data.Models;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}