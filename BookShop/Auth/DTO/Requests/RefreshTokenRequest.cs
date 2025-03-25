namespace BookShop.Auth.DTO.Requests;

public record RefreshTokenRequest(string Username, string RefreshToken);