namespace BookShop.ADMIN.DTOs;

public class ReviewDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string BookTitle { get; set; } = null!;
    public string Comment { get; set; } = null!; 
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
