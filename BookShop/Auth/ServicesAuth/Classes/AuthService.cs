using Microsoft.AspNetCore.Identity;
using BookShop.Data;
using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.DTOAuth.Responses;
using BookShop.Auth.ModelsAuth;
using System.Security.Claims;
using System.Security.Cryptography;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Auth.ServicesAuth.Classes
{
    public class AuthService : IAuthService
    {
        private readonly LibraryContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(LibraryContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Метод для регистрации пользователя
        public async Task RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
            };

            // Хэшируем пароль перед сохранением
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Метод для входа (логина) пользователя
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var accessToken = await _tokenService.CreateTokenAsync(claims);
            var refreshToken = GenerateRefreshToken();  // Генерация нового refresh токена

            user.RefreshToken = refreshToken;  // Сохраняем как строку
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            return new LoginResponse(accessToken, refreshToken);  // Возвращаем строковые токены
        }

        // Метод для обновления refresh токена
        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

            if (user == null || user.RefreshToken != request.RefreshToken)
            {
                throw new Exception("Invalid refresh token");
            }

            if (user.RefreshTokenExpiration < DateTime.Now)
            {
                throw new Exception("Refresh token has expired");
            }

            // Генерация нового refresh токена
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var newAccessToken = await _tokenService.CreateTokenAsync(claims);

            await _context.SaveChangesAsync();

            // Возвращаем новые токены
            return new RefreshTokenResponse(newAccessToken, newRefreshToken);
        }

        // Метод для генерации случайного и безопасного refresh токена
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; // 256 бит
            using (var rng = RandomNumberGenerator.Create()) // Используем RandomNumberGenerator
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber); // Генерируем refresh token в виде Base64 строки
        }
    }
}
