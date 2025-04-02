using BookShop.ADMIN.DTOs;

namespace BookShop.ADMIN.ServicesAdmin.WarehouseServices;

public interface IWarehouseService
{
    Task<List<WarehouseItemDto>> GetAllAsync();
    Task<bool> UpdateStockAsync(Guid bookId, int amount);
}