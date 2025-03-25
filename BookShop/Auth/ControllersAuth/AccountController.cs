using BookShop.Auth.DTO.Requests;
using BookShop.Auth.DTO.Responses;
using BookShop.Auth.Services.Interfaces;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Auth.ControllersAuth;

[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _accountService.RegisterAsync(request);

        if (result == null || !result.IsSuccess)
        {
            return BadRequest(new Result<string>(false, result?.Message ?? "Unknown error"));
        }

        return Ok(new Result<string>(true, result.Data, "Successfully registered"));
    }


    [HttpGet("VerifyEmail")]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] string token)
    {
        var result = await _accountService.VerifyEmailAsync(token);

        if (!result.Success)
        {
            return BadRequest(new Result<string>(false, result.Message));
        }

        return Ok(new Result<string>(true, "Email confirmed", "Email confirmed"));
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _accountService.LoginAsync(request);
        
        if (response == null)
        {
            return Unauthorized(new Result<LoginResponse>(false, "Invalid credentials"));
        }

        Response.Cookies.Append("accessToken", response.AccessToken, new CookieOptions { HttpOnly = true, Secure = true });
        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });

        return Ok(new Result<LoginResponse>(true, response, "Successfully logged in"));
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var accessToken = Request.Cookies["accessToken"];

        var newTokens = await _accountService.RefreshTokensAsync(accessToken, refreshToken);

        if (newTokens == null)
        {
            return Unauthorized(new Result<RefreshTokenResponse>(false, "Invalid refresh token"));
        }

        Response.Cookies.Append("accessToken", newTokens.AccessToken, new CookieOptions { HttpOnly = true, Secure = true });
        Response.Cookies.Append("refreshToken", newTokens.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });

        return Ok(new Result<RefreshTokenResponse>(true, newTokens, "Tokens refreshed successfully"));
    }
}
