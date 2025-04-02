using BookShop.ADMIN.DTOs;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly LibraryContext _context;

    public OrderService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                Username = o.User.UserName,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            Username = order.User.UserName,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };
    }

    public async Task<bool> UpdateStatusAsync(Guid id, string newStatus)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return false;

        if (!Enum.TryParse<Order.OrderStatus>(newStatus, true, out var parsedStatus))
            return false;

        order.Status = parsedStatus;
        await _context.SaveChangesAsync();
        return true;
    }
}