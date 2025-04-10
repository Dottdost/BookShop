using BookShop.ADMIN.DTOs;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Data.Contexts;

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

    public Task<bool> UpdateStockAsync(Guid bookId, int amount)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateStockAsync(int bookId, int amount)
    {
        // Находим склад по идентификатору книги
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.BookId == bookId);  // Используем int для идентификатора

        if (warehouse == null) return false;

        // Обновляем количество на складе (добавляем или убавляем в зависимости от amount)
        warehouse.Quantity += amount;

        // Обновляем время последнего изменения
        warehouse.UpdatedAt = DateTime.UtcNow;

        // Сохраняем изменения в базе данных
        await _context.SaveChangesAsync();

        return true;  // Возвращаем true, если обновление прошло успешно
    }

    
}
