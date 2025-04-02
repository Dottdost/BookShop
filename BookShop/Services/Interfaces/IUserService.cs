using BookShop.ADMIN.DTOs;

namespace BookShop.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}