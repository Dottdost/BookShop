using BookShop.ADMIN.DTOs;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;

namespace BookShop.ADMIN.ServicesAdmin.WarehouseServices;

public class WarehouseService : IWarehouseService
{
    private readonly LibraryContext _context;

    public WarehouseService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<WarehouseItemDto>> GetAllAsync()
    {
        return await _context.Warehouses
            .Include(w => w.Book)
            .Select(w => new WarehouseItemDto
            {
                BookId = w.BookId,
                Title = w.Book.Title,
                Quantity = w.Quantity,
                UpdatedAt = w.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(Guid bookId, int amount)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.BookId == bookId);

        if (warehouse == null) return false;

        warehouse.UpdateStock(amount);
        await _context.SaveChangesAsync();
        return true;
    }
}
