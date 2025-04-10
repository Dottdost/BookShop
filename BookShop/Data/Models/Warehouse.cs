namespace BookShop.Data.Models;

public class Warehouse
{
    public int Id { get; set; }
    public int BookId { get; set; } 
    public int Quantity { get; set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

    public Book Book { get; set; } = null!;


    // Метод для обновления количества и времени последнего изменения
    public void UpdateStock(int amount)
    {
        Quantity += amount;
        UpdatedAt = DateTime.UtcNow;
    }
}