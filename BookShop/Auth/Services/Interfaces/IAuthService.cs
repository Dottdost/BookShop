using BookShop.Auth.DTO.Requests;
using BookShop.Auth.DTO.Responses;

namespace BookShop.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}
