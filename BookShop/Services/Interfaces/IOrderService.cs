using BookShop.ADMIN.DTOs.OrderDto;
using BookShop.Data.Models;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO dto);
        Task<OrderResponseDTO> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersAsync(int page = 1, int pageSize = 20);
        Task<bool> DeleteOrderAsync(int orderId);
        
        Task<bool> UpdateOrderStatusAsync(int orderId, Order.OrderStatus status);
    }
}