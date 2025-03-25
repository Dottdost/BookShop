
namespace BookShop.Shared.DTO.Response;

public class BookResponseDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string GenreName { get; set; }
    public string PublisherName { get; set; }

    public BookResponseDTO(Guid id, string title, string author, decimal price, string description, string genreName, string publisherName)
    {
        Id = id;
        Title = title;
        Author = author;
        Price = price;
        Description = description;
        GenreName = genreName;
        PublisherName = publisherName;
    }
}

