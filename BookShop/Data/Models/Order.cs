using BookShop.Auth.ModelsAuth;

namespace BookShop.Data.Models;

public class Order
{
    public int Id { get; set; } 
    public int UserId { get; set; } 
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }

    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Completed,
        Canceled
    }
}