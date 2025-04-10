using BookShop.Data;
using BookShop.Data.Models;
using BookShop.ADMIN.DTOs;
using BookShop.Data.Contexts;
using BookShop.Shared.DTO.Response;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        // Создание новой книги
        public async Task<BookDto> CreateBookAsync(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                GenreId = dto.GenreId,
                PublisherId = dto.PublisherId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                GenreName = book.Genre?.GenreName,  
                PublisherName = book.Publisher?.Name 
            };

            return bookDto;
        }

        // Получение книги по ID
        public async Task<BookDto> GetBookAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)  
                .Include(b => b.Publisher)  
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                GenreName = book.Genre?.GenreName,
                PublisherName = book.Publisher?.Name
            };
        }

        // Получение списка книг с пагинацией
        public async Task<IEnumerable<BookDto>> GetBooksAsync(int page = 1, int pageSize = 20)
        {
            var books = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                GenreName = book.Genre?.GenreName,
                PublisherName = book.Publisher?.Name
            });
        }

        // Обновление информации о книге
        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Price = dto.Price;
            book.Stock = dto.Stock;
            book.Description = dto.Description;
            book.ImageUrl = dto.ImageUrl;
            book.GenreId = dto.GenreId;
            book.PublisherId = dto.PublisherId;

            await _context.SaveChangesAsync();

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                GenreName = book.Genre?.GenreName,
                PublisherName = book.Publisher?.Name
            };
        }

        // Удаление книги
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
