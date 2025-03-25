namespace BookShop.Data.Models;

public class BookAttribute
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}