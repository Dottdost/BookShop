namespace BookShop.ADMIN.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class UpdateOrderStatusDto
{
    public string Status { get; set; } = null!;
}
