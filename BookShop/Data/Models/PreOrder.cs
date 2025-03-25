namespace BookShop.Data.Models;

public class PreOrder
{
    public Guid Id { get; set; }
    public string UserName { get; set; } 
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; } 
}