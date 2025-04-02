using BookShop.Auth.DTOAuth.Requests;

namespace BookShop.Auth.ServicesAuth.Interfaces;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest request);
    Task ConfirmEmailAsync(ConfirmRequest request, HttpContext context);
    Task VerifyEmailAsync(string token);
}