using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;
using BookShop.ADMIN.DTOs.GenreDto;
using BookShop.Data.Contexts;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly LibraryContext _context;

        public GenreService(LibraryContext context)
        {
            _context = context;
        }

        // Создание нового жанра
        public async Task<GenreResponseDto> CreateGenreAsync(CreateGenreDTO dto)
        {
            var genre = new Genre
            {
                GenreName = dto.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            var response = new GenreResponseDto
            {
                Id = genre.Id,
                Name = genre.GenreName,
                ParentGenreId = null,  // Родительский жанр для нового жанра пока пуст
                ParentGenreName = null  // Родительский жанр для нового жанра пока пуст
            };

            return response;
        }

        // Получение жанра по имени
        public async Task<GenreResponseDto> GetGenreAsync(string name)
        {
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.GenreName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (genre == null)
            {
                return null;  // Возвращаем null, если жанр не найден
            }

            var parentGenreName = genre.ParentGenreId.HasValue
                ? await _context.Genres.Where(g => g.Id == genre.ParentGenreId).Select(g => g.GenreName).FirstOrDefaultAsync()
                : null;

            var response = new GenreResponseDto
            {
                Id = genre.Id,
                Name = genre.GenreName,
                ParentGenreId = genre.ParentGenreId,
                ParentGenreName = parentGenreName
            };

            return response;
        }

        // Получение всех жанров с пагинацией
        public async Task<IEnumerable<GenreResponseDto>> GetGenresAsync(int page = 1, int pageSize = 20)
        {
            var genres = await _context.Genres
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = genres.Select(genre =>
            {
                var parentGenreName = genre.ParentGenreId.HasValue
                    ? _context.Genres.Where(g => g.Id == genre.ParentGenreId).Select(g => g.GenreName).FirstOrDefault()
                    : null;

                return new GenreResponseDto
                {
                    Id = genre.Id,
                    Name = genre.GenreName,
                    ParentGenreId = genre.ParentGenreId,
                    ParentGenreName = parentGenreName
                };
            });

            return response;
        }

        // Обновление жанра
        public async Task<GenreResponseDto> UpdateGenreAsync(int id, UpdateGenreDto dto)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return null; // Жанр не найден
            }

            genre.GenreName = dto.Name;
            genre.ParentGenreId = dto.ParentGenreId;

            await _context.SaveChangesAsync();

            var parentGenreName = genre.ParentGenreId.HasValue
                ? await _context.Genres.Where(g => g.Id == genre.ParentGenreId).Select(g => g.GenreName).FirstOrDefaultAsync()
                : null;

            var response = new GenreResponseDto
            {
                Id = genre.Id,
                Name = genre.GenreName,
                ParentGenreId = genre.ParentGenreId,
                ParentGenreName = parentGenreName
            };

            return response;
        }

        // Удаление жанра
        public async Task<bool> DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return false; // Жанр не найден
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return true; // Успешное удаление
        }
    }
}
