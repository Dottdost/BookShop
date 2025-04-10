using Microsoft.AspNetCore.Mvc;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Models;
using BookShop.ADMIN.DTOs.GenreDto;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BookShop.Data.Contexts;

namespace BookShop.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly ILogger<GenreController> _logger;
        private readonly IMapper _mapper;

        public GenreController(LibraryContext context, ILogger<GenreController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // POST: api/genres/createParent
        [HttpPost("createParent")]
        public async Task<ActionResult<GenreDto>> CreateParentGenre(CreateGenreDto dto)
        {
            if (_context.Genres.Any(g => g.GenreName == dto.Name))
            {
                _logger.LogWarning("Attempted to create a parent genre with an existing name: {Name}", dto.Name);
                return BadRequest("Genre with this name already exists.");
            }

            var genre = new Genre
            {
                GenreName = dto.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Parent genre created successfully with name: {Name}", dto.Name);

            var genreDto = _mapper.Map<GenreDto>(genre);

            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genreDto);
        }

        // POST: api/genres/createSub
        [HttpPost("createSub")]
        public async Task<ActionResult<GenreDto>> CreateSubGenre(CreateSubGenreDto dto)
        {
            if (_context.Genres.Any(g => g.GenreName == dto.Name))
            {
                return BadRequest("A genre with this name already exists.");
            }

            var parentGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == dto.ParentGenreId);
            if (parentGenre == null)
            {
                return BadRequest("Parent genre not found.");
            }

            var subGenre = new Genre
            {
                GenreName = dto.Name,
                ParentGenre = parentGenre
            };

            _context.Genres.Add(subGenre);
            await _context.SaveChangesAsync();

            var subGenreDto = _mapper.Map<GenreDto>(subGenre);

            return CreatedAtAction(nameof(GetGenreById), new { id = subGenre.Id }, subGenreDto);
        }

        // GET: api/genres/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenreById(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.SubGenres)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                _logger.LogWarning("Genre with ID: {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Fetched genre with ID: {Id}", id);

            var genreDto = _mapper.Map<GenreDto>(genre);
            return genreDto;
        }

        // DELETE: api/genres/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.SubGenres)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                _logger.LogWarning("Genre with ID: {Id} not found.", id);
                return NotFound("Genre not found.");
            }

            if (genre.SubGenres.Any()) // Если есть поджанры
            {
                _logger.LogWarning("Cannot delete genre with id {Id} because it has subgenres.", id);
                return BadRequest("Cannot delete genre with subgenres.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Genre with ID: {Id} deleted successfully.", id);
            return NoContent();
        }

        // DELETE: api/genres/deleteSub/{name}
        [HttpDelete("deleteSub/{name}")]
        public async Task<IActionResult> DeleteSubGenre(string name)
        {
            var subGenre = await _context.Genres
                .FirstOrDefaultAsync(g => g.GenreName == name);

            if (subGenre == null)
            {
                _logger.LogWarning("Subgenre with name: {Name} not found.", name);
                return NotFound("Subgenre not found.");
            }

            if (subGenre.SubGenres.Any()) // Если есть поджанры у поджанра
            {
                _logger.LogWarning("Cannot delete subgenre with name {Name} because it has subgenres.", name);
                return BadRequest("Cannot delete subgenre with subgenres.");
            }

            _context.Genres.Remove(subGenre);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Subgenre with name: {Name} deleted successfully.", name);
            return NoContent();
        }
    }
}
