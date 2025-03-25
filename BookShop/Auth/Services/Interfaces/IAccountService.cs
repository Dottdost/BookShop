using BookShop.Auth.DTO.Requests;

namespace BookShop.Auth.Services.Interfaces;

public interface IAccountService
{
    Task RegisterAsync(RegisterRequest request);
    Task ConfirmEmailAsync(ConfirmRequest request, HttpContext context);
    Task VerifyEmailAsync(string token);
}
