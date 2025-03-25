using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Services.Interfaces;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly LibraryContext _context;

        public GenreService(LibraryContext context)
        {
            _context = context;
        }
        // Создание жанра
        public async Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO dto)
        {
            var genre = new Genre
            {
                Name = dto.Name,
                ParentGenreId = dto.ParentGenreId
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            
            string? parentGenreName = null;
            if (genre.ParentGenreId.HasValue)
            {
                var parentGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.Id == genre.ParentGenreId.Value);
                parentGenreName = parentGenre?.Name;
            }
            
            var subGenres = await _context.Genres
                .Where(g => g.ParentGenreId == genre.Id)
                .Select(sg => new GenreSubGenreDTO(sg.Id, sg.Name))
                .ToListAsync();

            return new GenreResponseDTO(
                genre.Id,
                genre.Name,
                genre.ParentGenreId,
                parentGenreName,
                subGenres
            );
        }
        
        public async Task<GenreResponseDTO> GetGenreAsync(string name)
        {
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (genre == null)
                throw new Exception("Genre not found");

            // Получение имени родительского жанра
            string? parentGenreName = null;
            if (genre.ParentGenreId.HasValue)
            {
                var parentGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.Id == genre.ParentGenreId.Value);
                parentGenreName = parentGenre?.Name;
            }

            // Получение поджанров
            var subGenres = await _context.Genres
                .Where(g => g.ParentGenreId == genre.Id)
                .Select(sg => new GenreSubGenreDTO(sg.Id, sg.Name))
                .ToListAsync();

            return new GenreResponseDTO(
                genre.Id,
                genre.Name,
                genre.ParentGenreId,
                parentGenreName,
                subGenres
            );
        }
        
        public async Task<IEnumerable<GenreResponseDTO>> GetGenresAsync(int page = 1, int pageSize = 20)
        {
            var genres = await _context.Genres
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Для каждого жанра получаем родительский жанр и поджанры
            var genreResponses = new List<GenreResponseDTO>();

            foreach (var genre in genres)
            {
                string? parentGenreName = null;
                if (genre.ParentGenreId.HasValue)
                {
                    var parentGenre = await _context.Genres
                        .FirstOrDefaultAsync(g => g.Id == genre.ParentGenreId.Value);
                    parentGenreName = parentGenre?.Name;
                }
                
                var subGenres = await _context.Genres
                    .Where(g => g.ParentGenreId == genre.Id)
                    .Select(sg => new GenreSubGenreDTO(sg.Id, sg.Name))
                    .ToListAsync();

                genreResponses.Add(new GenreResponseDTO(
                    genre.Id,
                    genre.Name,
                    genre.ParentGenreId,
                    parentGenreName,
                    subGenres
                ));
            }

            return genreResponses;
        }
    }
}
