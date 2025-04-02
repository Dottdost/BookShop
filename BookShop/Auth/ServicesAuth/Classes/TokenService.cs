using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.Auth.ServicesAuth.Classes;

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
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value
                   ?? throw new SecurityTokenException("Username not found in token");
        }
        catch (Exception)
        {
            throw new SecurityTokenException("Invalid token");
        }
    }

    // Создание токена для пользователя
    public async Task<string> CreateTokenAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
            throw new SecurityTokenException("Invalid username");

        var userRoles = await _context.UserRoles
            .Where(u => u.UserId == user.Id)
            .Select(u => u.Role.RoleName)
            .AsNoTracking()
            .ToListAsync();

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtKey = _config["JWT:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT:Key is not configured");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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
