using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Services.Interfaces;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        // Создание книги
        public async Task<BookResponseDTO> CreateProductAsync(CreateBookDTO dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Price = dto.Price,
                GenreId = dto.GenreId,
                PublisherId = dto.PublisherId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return new BookResponseDTO(
                book.Id,
                book.Title,
                book.Author,
                book.Price,
                book.Description ?? "Описание отсутствует",
                book.GenreId.ToString(),
                book.PublisherId.ToString()
            );
        }

        // Получение книги по имени (по автору или названию)
        public async Task<IEnumerable<BookResponseDTO>> GetProductAsync(string name)
        {
            var books = await _context.Books
                .Where(b => b.Title.Contains(name) || b.Author.Contains(name))
                .Select(b => new BookResponseDTO(
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Price,
                    b.Description ?? "Описание отсутствует",
                    b.GenreId.ToString(),
                    b.PublisherId.ToString()
                ))
                .ToListAsync();

            return books;
        }

        // Получение списка книг с пагинацией
        public async Task<IEnumerable<BookResponseDTO>> GetProductsAsync(int page = 1, int pageSize = 20)
        {
            var booksQuery = _context.Books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookResponseDTO(
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Price,
                    b.Description ?? "Описание отсутствует",
                    b.GenreId.ToString(),
                    b.PublisherId.ToString()
                ));

            return await booksQuery.ToListAsync();
        }
    }
}
