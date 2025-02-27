namespace BookShop.Data.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = ""; //нужно написать статусы сюда, думаю
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public User User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}