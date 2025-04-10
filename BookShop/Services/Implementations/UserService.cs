using System.Security.Cryptography;
using System.Text;
using BookShop.ADMIN.DTOs;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Auth.ModelsAuth;
using BookShop.Data.Contexts;
using BookShop.Shared.DTO.Response;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static BookShop.ADMIN.DTOs.UserDto;

namespace BookShop.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly LibraryContext _context;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        // Создание нового пользователя
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)  
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        // Получение информации о пользователе по ID
        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        // Получение списка пользователей с пагинацией
        public async Task<IEnumerable<UserDto>> GetUsersAsync(int page = 1, int pageSize = 20)
        {
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        // Обновление данных пользователя
        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = HashPassword(dto.Password);  
            }

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        // Удаление пользователя
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
