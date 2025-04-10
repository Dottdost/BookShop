namespace BookShop.Data.Models;

public class BookAttributeValue
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;

    public int BookId { get; set; } 
    public Book Book { get; set; } = null!;

    public int AttributeId { get; set; } 
    public BookAttribute Attribute { get; set; } = null!;
}