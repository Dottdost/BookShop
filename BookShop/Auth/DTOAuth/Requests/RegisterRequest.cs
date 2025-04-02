namespace BookShop.Auth.DTOAuth.Requests;

public record RegisterRequest(string Username, string Password, string ConfirmPassword, string Email);