namespace BookShop.ADMIN.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string GenreName { get; set; }  // Название жанра
        public string PublisherName { get; set; }  // Название издателя
    }

    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int GenreId { get; set; }
        public int? PublisherId { get; set; }
    }

    public class UpdateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int GenreId { get; set; }
        public int? PublisherId { get; set; }
    }
}