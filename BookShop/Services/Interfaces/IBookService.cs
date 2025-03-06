using Shared.DTO.Response;
using System.Threading.Tasks;
using System.Collections.Generic;
using BookShop.Shared.DTO.Requests;

namespace BookShop.Services.Interfaces;

public interface IBookService
{
    Task<BookResponseDTO> CreateBookAsync(CreateBookDTO dto);
    Task<BookResponseDTO> GetBookByIdAsync(Guid id);
    Task<IEnumerable<BookResponseDTO>> GetBooksAsync(int page = 1, int pageSize = 20);
}