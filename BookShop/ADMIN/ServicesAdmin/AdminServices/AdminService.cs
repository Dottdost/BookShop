using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using BookShop.ADMIN.DTOs.DTOAdmin;
using BookShop.ADMIN.ModelsAdmin;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.ADMIN.ServicesAdmin.AdminServices;

public class AdminService : IAdminService
{
    private readonly LibraryContext _context;
    private readonly IConfiguration _config;

    public AdminService(LibraryContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<bool> RegisterAsync(AdminRegisterDto dto)
    {
        if (await _context.Admins.AnyAsync(a => a.Username == dto.Username || a.Email == dto.Email))
            return false;

        var hash = HashPassword(dto.Password);
        var admin = new Admin
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hash
        };

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> LoginAsync(AdminLoginDto dto)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == dto.Username);
        if (admin == null || admin.PasswordHash != HashPassword(dto.Password))
            throw new UnauthorizedAccessException();

        // Генерация JWT
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
