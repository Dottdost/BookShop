namespace BookShop.ADMIN.DTOs.OrderDto
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
