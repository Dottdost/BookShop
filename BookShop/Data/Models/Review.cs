using System.ComponentModel.DataAnnotations;
using BookShop.Auth.ModelsAuth;
using BookShop.Data.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public Guid BookId { get; set; }

    [Range(1, 5, ErrorMessage = "Range has to be between 1 and 5")]
    public int Rating { get; set; }

    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
}