using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookShop.Auth.Services.Interfaces;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.Auth.Services.Classes;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly LibraryContext _context;

    public TokenService(IConfiguration config, LibraryContext context)
    {
        _config = config;
        _context = context;
    }

    // Получаем имя пользователя из токена
    public async Task<string> GetNameFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (securityToken == null)
            throw new SecurityTokenException("Invalid token");

        var username = securityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

        return username?.Value;
    }

    // Создание токена для пользователя
    public async Task<string> CreateTokenAsync(string username)
    {
        // Получаем пользователя по имени
        var user = await _context.Users
            .Where(u => u.UserName == username)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new UnauthorizedAccessException("Invalid username");

        // Получаем роли пользователя
        var userRoles = _context.UserRoles
            .Where(u => u.UserId == user.Id)  // Используем Id пользователя для связи с ролями
            .Select(u => u.Role.RoleName)  // Предположим, что у Role есть свойство Name
            .AsNoTracking()
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
        };

        // Добавляем роли в claims
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Генерация токена
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            signingCredentials: signingCred
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}