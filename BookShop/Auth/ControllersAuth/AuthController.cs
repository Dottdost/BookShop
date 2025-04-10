using System.Security.Claims;
using System.Security.Cryptography;
using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.DTOAuth.Responses;
using BookShop.Auth.ModelsAuth;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using BookShop.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Auth.ControllersAuth
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly LibraryContext _context;  

        public AuthController(IAuthService authService, ITokenService tokenService, LibraryContext context)
        {
            _tokenService = tokenService;
            _authService = authService;
            _context = context; 
        }

        // Login
        [HttpPost("Login")]
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

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            return new LoginResponse(accessToken, refreshToken);  // Используем record для возвращаемого значения
        }

        // Refresh Token
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var accessToken = Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { message = "Invalid tokens" });
            }

            try
            {
                var request = new RefreshTokenRequest(await _tokenService.GetNameFromToken(accessToken), refreshToken);
                var newTokens = await _authService.RefreshTokenAsync(request);

                SetTokensInCookies(newTokens.AccessToken, newTokens.RefreshToken);

                return Ok(new Result<RefreshTokenResponse>(true, newTokens, "Successfully refreshed token"));
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }
        }

        // Logout
        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Successfully logged out" });
        }

        // Test
        [HttpPost("Test")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Test()
        {
            return Ok("Test successful");
        }

        // Helper method to set tokens in cookies
        private void SetTokensInCookies(string accessToken, string refreshToken)
        {
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }

        // Метод для генерации refresh токена
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; 
            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber); // Генерируем refresh token в виде Base64 строки
        }//заменить потом на ша 256
    }
}
