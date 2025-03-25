namespace BookShop.Data.Models;

public class BookAttributeValue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;

    public int BookId { get; set; } 
    public Book Book { get; set; } = null!;

    public Guid AttributeId { get; set; } 
    public BookAttribute Attribute { get; set; } = null!;
}