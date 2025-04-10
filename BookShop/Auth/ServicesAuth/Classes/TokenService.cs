using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using BookShop.Data.Contexts;

namespace BookShop.Auth.ServicesAuth.Classes
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly LibraryContext _context;

        public TokenService(IConfiguration config, LibraryContext context)
        {
            _config = config;
            _context = context;
        }

        // Метод для создания токена с возможностью настройки времени жизни
        public async Task<string> CreateTokenAsync(List<Claim> claims, int expirationMinutes = 15)
        {
            var jwtKey = _config["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT:Key is not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes), // Используем параметр для времени истечения
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                signingCredentials: signingCred
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(securityToken));  // Возвращаем токен
        }

        // Метод для получения имени пользователя из токена
        public async Task<string> GetNameFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["JWT:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  // Без допуска по времени для истечения токена
                }, out var validatedToken);

                // Извлекаем имя пользователя
                var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    throw new SecurityTokenException("Username not found in token");
                }
                
                return username;
            }
            catch (Exception ex)
            {
                // Логируем или обрабатываем ошибку по вашему усмотрению
                throw new SecurityTokenException($"Invalid token: {ex.Message}");
            }
        }
    }
}
