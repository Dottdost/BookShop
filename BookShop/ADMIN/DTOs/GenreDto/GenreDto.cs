namespace BookShop.ADMIN.DTOs.GenreDto;

public class GenreDto
{
    public int Id { get; set; }
    public string GenreName { get; set; }
    public int? ParentGenreId { get; set; }
    public List<GenreDto> SubGenres { get; set; }
    public List<UpdateBookDto> Books { get; set; }  
}