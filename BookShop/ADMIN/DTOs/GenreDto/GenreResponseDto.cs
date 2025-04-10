namespace BookShop.ADMIN.DTOs.GenreDto
{
    public class GenreResponseDto
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public int? ParentGenreId { get; set; }
        public string ParentGenreName { get; set; }  

        public GenreResponseDto() { }

        
        public GenreResponseDto(int id, string name, int? parentGenreId, string parentGenreName, IEnumerable<GenreSubGenreDTO> subGenres)
        {
            Id = id;
            Name = name;
            ParentGenreId = parentGenreId;
            ParentGenreName = parentGenreName;
            SubGenres = subGenres;
        }

        public IEnumerable<GenreSubGenreDTO> SubGenres { get; set; }  
    }
}