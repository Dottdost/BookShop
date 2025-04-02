namespace BookShop.ADMIN.DTOs;


public class WarehouseItemDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateWarehouseStockDto
{
    public int Amount { get; set; } // Можно отрицательное — для вычета
}