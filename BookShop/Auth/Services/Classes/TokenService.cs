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

    public async Task<string> GetNameFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (securityToken == null)
            throw new SecurityTokenException("Invalid token");

        var username = securityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

        return username.Value;
    }

    public async Task<string> CreateTokenAsync(string username)
    {
        var userRoles = _context.UserRoles.Where(u => u.UserNameRef == username)
            .Select(u => new { Role = u.RoleNameRef })
            .AsNoTracking();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            signingCredentials: signingCred);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
