using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookShop.ADMIN.DTOs.OrderDto;
using BookShop.Data.Models;

namespace BookShop.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Создание нового заказа
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDTO createOrderDTO)
        {
            if (createOrderDTO == null)
            {
                return BadRequest("Invalid order data.");
            }

            var order = await _orderService.CreateOrderAsync(createOrderDTO);
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { orderId = order.Id }, order);
        }

        // Получение заказа по ID
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(order);
        }

        // Получение всех заказов с пагинацией
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var orders = await _orderService.GetOrdersAsync(page, pageSize);
            return Ok(orders);
        }

        // Обновление статуса заказа
        [HttpPut("UpdateStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, [FromBody] Order.OrderStatus status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);

            if (!result)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(new { message = "Order status updated successfully." });
        }

        // Удаление заказа
        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<IActionResult> DeleteOrderAsync(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);

            if (!result)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(new { message = "Order deleted successfully." });
        }
    }
}
