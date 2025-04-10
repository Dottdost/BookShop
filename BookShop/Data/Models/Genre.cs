namespace BookShop.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }  // Используем int вместо Guid для ID жанра
        public string GenreName { get; set; } = string.Empty;

        public int? ParentGenreId { get; set; }  // Связь с родительским жанром через int
        public Genre? ParentGenre { get; set; }

        public ICollection<Genre> SubGenres { get; set; } = new List<Genre>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}