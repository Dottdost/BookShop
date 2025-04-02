using BookShop.ADMIN.DTOs.DTOAdmin;

namespace BookShop.ADMIN.ServicesAdmin.AdminServices;

// Services/IAdminService.cs
public interface IAdminService
{
    Task<string> LoginAsync(AdminLoginDto dto);
    Task<bool> RegisterAsync(AdminRegisterDto dto);
}
