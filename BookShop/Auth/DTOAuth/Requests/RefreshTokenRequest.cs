namespace BookShop.Auth.DTOAuth.Requests;

public record RefreshTokenRequest(string Username, string RefreshToken);