namespace BookShop.Data.Models;
//просто дополнительная информация информация 
public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } 
    
    public Order Order { get; set; } 
    public Book Book { get; set; }
}