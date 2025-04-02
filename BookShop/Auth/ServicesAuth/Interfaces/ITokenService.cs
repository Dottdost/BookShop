namespace BookShop.Auth.ServicesAuth.Interfaces;

public interface ITokenService
{
    Task<string> GetNameFromToken(string token);
    Task<string> CreateTokenAsync(string username);
}