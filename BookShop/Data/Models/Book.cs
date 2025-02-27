namespace BookShop.Data.Models;

public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } 
    public string ImageUrl { get; set; } 
    public Guid CategoryId { get; set; }
    
    public Category Category { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}