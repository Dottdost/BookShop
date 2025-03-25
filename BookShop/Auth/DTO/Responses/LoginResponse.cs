namespace BookShop.Auth.DTO.Responses;

public record LoginResponse(string AccessToken, string RefreshToken);