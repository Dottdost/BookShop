using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.DTOAuth.Responses;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Auth.ServicesAuth.Classes;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly LibraryContext _context;

    public AuthService(LibraryContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var accessToken = await _tokenService.CreateTokenAsync(request.Username);
        var refreshToken = Guid.NewGuid();
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = DateTime.Now.AddDays(7);
        
        await _context.SaveChangesAsync();
        
        return new LoginResponse(accessToken, refreshToken.ToString());
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
        
        if (user == null || user.RefreshToken.ToString() != request.RefreshToken)
            throw new Exception("Invalid refresh token");

        user.RefreshToken = Guid.NewGuid();
        user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

        await _context.SaveChangesAsync();

        return new RefreshTokenResponse(await _tokenService.CreateTokenAsync(request.Username), user.RefreshToken.ToString());
    }
}