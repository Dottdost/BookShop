using BookShop.Auth.ModelsAuth;
using BookShop.Data.Models;

public class User
{
    public int Id { get; set; }  // Используем int, если хотим использовать целочисленные идентификаторы
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; } = DateTime.Now.AddDays(7);
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}