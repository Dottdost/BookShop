namespace BookShop.Data.Models;

public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } 
    public decimal Discount { get; set; }  // скидка которую промокод даст
    public DateTime ExpiryDate { get; set; }  // cрок 
    public bool IsActive { get; set; } 
    
    public ICollection<Order> Orders { get; set; }
}