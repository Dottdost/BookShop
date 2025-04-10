using AutoMapper;
using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.ModelsAuth;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using BookShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookShop.Auth.DTOAuth.Responses;
using BookShop.Data.Contexts;

namespace BookShop.Auth.ServicesAuth.Classes
{
    public class AccountService : IAccountService
    {
        private readonly LibraryContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService; // Для отправки email

        public AccountService(LibraryContext context, ITokenService tokenService, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
            _emailService = emailService;
        }

        // Метод для регистрации пользователя
        public async Task<Result<string>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Проверка на уникальность имени пользователя и email
                if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
                {
                    return Result<string>.Error(null, "Username already exists");
                }

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return Result<string>.Error(null, "Email already exists");
                }

                var user = new User
                {
                    UserName = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                };

                // Добавление пользователя в базу данных
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "User")
                };

                var token = await _tokenService.CreateTokenAsync(claims);

                return Result<string>.Success(token, "Successfully registered");
            }
            catch (Exception ex)
            {
                // Логирование ошибки для подробного анализа
                // Логируем исключение, чтобы выявить проблему
                Console.WriteLine($"Error during registration: {ex.Message}");
                return Result<string>.Error(null, $"Error during registration: {ex.Message}");
            }
        }

        // Метод для хэширования пароля
        public string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(null, password);
        }

        // Метод для запроса сброса пароля
        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Генерация токена для сброса пароля
            var token = Guid.NewGuid().ToString();

            // Отправка email с ссылкой для сброса пароля
            var resetLink = $"{_config["AppUrl"]}/reset-password?token={token}";
            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", 
                $"Please click the following link to reset your password: {resetLink}");

            // Сохранение токена в базе данных
            user.RefreshToken = token;
            user.RefreshTokenExpiration = DateTime.Now.AddHours(1); // Токен действителен 1 час
            await _context.SaveChangesAsync();
        }

        // Метод для сброса пароля
        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
            if (user == null || user.RefreshTokenExpiration < DateTime.Now)
            {
                throw new Exception("Invalid or expired token");
            }

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            // Обновляем информацию о пользователе
            user.RefreshToken = null;
            user.RefreshTokenExpiration = DateTime.Now.AddMinutes(-1); // Устанавливаем истекший токен
            await _context.SaveChangesAsync();

        }

        // Метод для подтверждения email
        public async Task ConfirmEmailAsync(ConfirmRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Генерация уникального токена для подтверждения email
            var token = Guid.NewGuid().ToString();

            // Сохранение токена в базе данных (например, в поле RefreshToken)
            user.RefreshToken = token;
            user.RefreshTokenExpiration = DateTime.Now.AddHours(1);  // Время жизни токена — 1 час
            await _context.SaveChangesAsync();

            // Формирование ссылки для подтверждения
            var confirmationLink = $"{_config["AppUrl"]}/confirm-email?token={token}";

            // Отправка email с подтверждением
            await _emailService.SendEmailAsync(user.Email, "Confirm your email", 
                $"Please click the following link to confirm your email: <a href='{confirmationLink}'>Confirm Email</a>");
        }

        // Метод для верификации email по токену
        public async Task VerifyEmailAsync(string token)
        {
            var username = await _tokenService.GetNameFromToken(token);
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Invalid token");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Подтверждаем email пользователя
            user.IsEmailConfirmed = true;
            user.RefreshToken = null;  // Убираем токен после подтверждения
            user.RefreshTokenExpiration = DateTime.Now.AddMinutes(-1); // Устанавливаем истекший токен
          

            await _context.SaveChangesAsync();
        }
    }
}
