using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.DTOAuth.Responses;

public interface IAccountService
{
    Task<Result<string>> RegisterAsync(RegisterRequest request);
    Task ConfirmEmailAsync(ConfirmRequest request);  
    Task VerifyEmailAsync(string token);

    // Методы для сброса пароля
    Task RequestPasswordResetAsync(string email);  // Запрос на сброс пароля
    Task ResetPasswordAsync(string token, string newPassword);  // Подтверждение сброса пароля
}