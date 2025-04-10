using BookShop.ADMIN.DTOs.DTOAdmin;
using BookShop.ADMIN.ServicesAdmin.AdminServices;

namespace BookShop.ADMIN.ControllersAdmin;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto dto)
    {
        try
        {
            var token = await _adminService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch
        {
            return Unauthorized("Invalid username or password");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
    {
        var result = await _adminService.RegisterAsync(dto);
        if (!result)
            return BadRequest("This admin already exists");
        return Ok("Admin created");
    }
}
