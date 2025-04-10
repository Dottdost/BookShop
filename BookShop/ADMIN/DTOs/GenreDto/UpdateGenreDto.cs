namespace BookShop.ADMIN.DTOs.GenreDto
{
    public class UpdateGenreDto
    {
        public string Name { get; set; } 
        public int? ParentGenreId { get; set; }  
    }
    
    public class UpdateSubGenreDto
    {
        public string Name { get; set; }  
        public int? ParentGenreId { get; set; }  
    }
}