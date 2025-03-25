using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO dto);
        
        Task<GenreResponseDTO> GetGenreAsync(string name);
        
        Task<IEnumerable<GenreResponseDTO>> GetGenresAsync(int page = 1, int pageSize = 20);
    }
}