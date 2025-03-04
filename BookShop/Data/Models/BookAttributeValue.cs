namespace BookShop.Data.Models;

public class BookAttributeValue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } // Например: "Русский", "Твердый переплет"
    
    public Guid BookId { get; set; }
    public Book Book { get; set; }

    public Guid AttributeId { get; set; }
    public Attribute Attribute { get; set; }
}