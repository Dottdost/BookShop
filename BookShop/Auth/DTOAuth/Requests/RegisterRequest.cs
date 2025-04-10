namespace BookShop.Auth.DTOAuth.Requests
{
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty; // Убедитесь, что оно инициализируется не null
    public string Password { get; set; } = string.Empty; // Убедитесь, что оно инициализируется не null
    public string ConfirmPassword { get; set; } = string.Empty; // Убедитесь, что оно инициализируется не null
    public string Email { get; set; } = string.Empty; // Убедитесь, что оно инициализируется не null
}
}
