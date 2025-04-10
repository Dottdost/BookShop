namespace BookShop.Data.Models;

public class BookAttribute
{
    public int Id { get; set; } 
    public string Name { get; set; } = string.Empty;

    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}