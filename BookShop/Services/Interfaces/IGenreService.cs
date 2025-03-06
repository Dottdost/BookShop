
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces;

public interface IGenreService
{
    Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO dto);
    Task<GenreResponseDTO> GetGenreAsync(Guid id);
    Task<PaginatedResponse<GenreResponseDTO>> GetGenresAsync(int page = 1, int pageSize = 20);
}