using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Interfaces
{
    public interface IBookService
    {
        
        
        public Task<BookResponseDTO> CreateProductAsync(CreateBookDTO dto);
        public Task<IEnumerable<BookResponseDTO>> GetProductAsync(string name);
        public Task<IEnumerable<BookResponseDTO>> GetProductsAsync(int page = 1, int pageSize = 20);
        
        
    }
    
}