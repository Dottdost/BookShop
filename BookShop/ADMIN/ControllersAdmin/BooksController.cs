using BookShop.ADMIN.DTOs;
using BookShop.Data;
using BookShop.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ADMIN.ControllersAdmin
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(int page = 1, int pageSize = 10, string search = "")
        {
            var query = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Where(b => b.Title.Contains(search) || b.Author.Contains(search)); // Поиск по названию и автору

            var books = await query
                .Skip((page - 1) * pageSize)  // Пагинация
                .Take(pageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    Stock = b.Stock,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl,
                    GenreName = b.Genre.GenreName,
                    PublisherName = b.Publisher.Name
                })
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                GenreName = book.Genre.GenreName,
                PublisherName = book.Publisher.Name
            };

            return Ok(bookDto);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] CreateBookDto dto)
        {
            var genre = await _context.Genres.FindAsync(dto.GenreId);
            if (genre == null)
            {
                return BadRequest(new { message = "Genre not found" });
            }

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                GenreId = dto.GenreId,
                GenreName = genre.GenreName,
                PublisherId = dto.PublisherId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            existingBook.Title = dto.Title;
            existingBook.Author = dto.Author;
            existingBook.Price = dto.Price;
            existingBook.Stock = dto.Stock;
            existingBook.Description = dto.Description;
            existingBook.ImageUrl = dto.ImageUrl;
            existingBook.GenreId = dto.GenreId;
            existingBook.PublisherId = dto.PublisherId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
