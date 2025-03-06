namespace BookShop.Data.Models;

public class Genre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    
    public Guid? ParentGenreId { get; set; }=Guid.NewGuid();
    public Genre? ParentGenre { get; set; }

    // Поджанры 
    public ICollection<Genre> SubGenres { get; set; } = new List<Genre>();

    public ICollection<Book> Books { get; set; } = new List<Book>();
}