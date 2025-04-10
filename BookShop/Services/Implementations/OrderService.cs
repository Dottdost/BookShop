using BookShop.ADMIN.DTOs.OrderDto;
using BookShop.Data;
using BookShop.Data.Contexts;
using BookShop.Data.Models;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly LibraryContext _context;

        public OrderService(LibraryContext context)
        {
            _context = context;
        }

        // Создание нового заказа
        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                TotalPrice = dto.TotalPrice,
                Status = Order.OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                OrderItems = dto.OrderItems.Select(item => new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString()
            };
        }

        // Получение заказа по ID
        public async Task<OrderResponseDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            return new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }

        // Получение всех заказов с пагинацией
        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersAsync(int page = 1, int pageSize = 20)
        {
            var orders = await _context.Orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }

        // Получение всех заказов по UserId с пагинацией
        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId, int page = 1, int pageSize = 20)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(o => o.OrderItems)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }

        // Обновление статуса заказа
        public async Task<bool> UpdateOrderStatusAsync(int orderId, Order.OrderStatus status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        // Удаление заказа
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
