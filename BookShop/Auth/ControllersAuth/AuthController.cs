using BookShop.Auth.DTO.Requests;
using BookShop.Auth.DTO.Responses;
using BookShop.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Auth.ControllersAuth;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        if (response == null)
        {
            return Unauthorized(new Result<LoginResponse>(false, "Invalid credentials"));
        }

        Response.Cookies.Append("accessToken", response.AccessToken);
        Response.Cookies.Append("refreshToken", response.RefreshToken);
        
        return Ok(new Result<LoginResponse>(true, response, "Successfully logged in"));
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var accessToken = Request.Cookies["accessToken"];
        
        var request = new RefreshTokenRequest(await _tokenService.GetNameFromToken(accessToken), refreshToken);
        
        var newTokens = await _authService.RefreshTokenAsync(request);
        
        if (newTokens == null)
        {
            return Unauthorized(new Result<RefreshTokenResponse>(false, "Invalid refresh token"));
        }
        
        Response.Cookies.Append("accessToken", newTokens.AccessToken);
        Response.Cookies.Append("refreshToken", newTokens.RefreshToken);
        
        return Ok(new Result<RefreshTokenResponse>(true, newTokens, "Successfully refreshed token"));
    }

    [HttpPost("Test")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult Test()
    {
        return Ok("Test");
    }
    
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // Clear cookies or handle logout logic here
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        return Ok("Successfully logged out");
    }
}
