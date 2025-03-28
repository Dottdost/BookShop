using BookShop.Data.Models;

namespace BookShop.Auth.Models;

public class Role
{
    public Guid RoleId { get; set; }  // Изменено с int на Guid
    public string RoleName { get; set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}