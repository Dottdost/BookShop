namespace BookShop.Data.Models;

public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid(); 
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public Guid GenreId { get; set; } 
    public Guid? PublisherId { get; set; }
    
    public string GenreName { get; set; }
    public string PublisherName { get; set; }

    public Genre Genre { get; set; } = null!;
    public Publisher? Publisher { get; set; } = null!;
    public Warehouse? Warehouse { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}