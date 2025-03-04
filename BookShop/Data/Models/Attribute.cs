using BookShop.Data.Models;

namespace BookShop.Data.FluentConfigs;

public class Attribute
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } 
    public ICollection<BookAttributeValue> BookAttributeValues { get; set; } = new List<BookAttributeValue>();
}