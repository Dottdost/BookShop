using BookShop.ADMIN.DTOs.GenreDto;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreResponseDto> CreateGenreAsync(CreateGenreDTO dto);
        Task<GenreResponseDto> GetGenreAsync(string name);
        Task<IEnumerable<GenreResponseDto>> GetGenresAsync(int page = 1, int pageSize = 20);
        Task<GenreResponseDto> UpdateGenreAsync(int id, UpdateGenreDto dto);
        Task<bool> DeleteGenreAsync(int id);
    }
}