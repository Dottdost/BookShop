namespace BookShop.Auth.DTOAuth.Responses;

public record LoginResponse(string AccessToken, string RefreshToken);