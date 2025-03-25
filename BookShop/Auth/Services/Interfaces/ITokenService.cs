namespace BookShop.Auth.Services.Interfaces;

public interface ITokenService
{
    Task<string> GetNameFromToken(string token);
    Task<string> CreateTokenAsync(string username);
}
