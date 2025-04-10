using BookShop.Data.Models;

public class Book
{
    public int Id { get; set; }  // ID книги, например, int
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public int GenreId { get; set; }  // ID жанра
    public int? PublisherId { get; set; }  // Используем int для PublisherId
    public string GenreName { get; set; } = string.Empty;
    public string PublisherName { get; set; } = string.Empty;

    public Genre Genre { get; set; } = null!;
    public Publisher? Publisher { get; set; }  // Связь с Publisher
    public Warehouse? Warehouse { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}