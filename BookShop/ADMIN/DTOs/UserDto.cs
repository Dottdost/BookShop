namespace BookShop.ADMIN.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}