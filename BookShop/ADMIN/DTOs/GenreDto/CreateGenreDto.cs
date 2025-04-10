namespace BookShop.ADMIN.DTOs.GenreDto
{
    public class CreateGenreDto
    {
        public string Name { get; set; }  
    }

    public class CreateSubGenreDto
    {
        public string Name { get; set; }  
        public int ParentGenreId { get; set; } 
    }
}