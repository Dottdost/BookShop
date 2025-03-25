namespace BookShop.Data.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public Guid BookId { get; set; }
    public int Rating { get; set; } //нужно добавить ограничение, 1-5
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public User User { get; set; } 
    public Book Book { get; set; } 
}