namespace BookShop.Data.Models;

public class Warehouse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public int Quantity { get; set; } // Количество в наличии

    public Book Book { get; set; } 
}