using BookShop.ADMIN.DTOs;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateBookAsync(CreateBookDto dto);
        Task<BookDto> GetBookAsync(int id);
        Task<IEnumerable<BookDto>> GetBooksAsync(int page = 1, int pageSize = 20);
        Task<BookDto> UpdateBookAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteBookAsync(int id);
    }
}