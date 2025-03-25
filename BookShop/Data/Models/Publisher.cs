namespace BookShop.Data.Models;

public class Publisher
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; } 
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}