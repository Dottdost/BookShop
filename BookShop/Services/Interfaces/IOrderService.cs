using BookShop.ADMIN.DTOs;

namespace BookShop.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateStatusAsync(Guid id, string newStatus);
}