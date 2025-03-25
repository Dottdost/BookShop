namespace BookShop.Auth.DTO.Responses;

public record RefreshTokenResponse(string AccessToken, string RefreshToken);