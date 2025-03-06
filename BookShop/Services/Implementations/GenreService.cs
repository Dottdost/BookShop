using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Services.Interfaces;
using BookShop.Shared.DTO.Requests;
using BookShop.Shared.DTO.Response;

namespace BookShop.Services.Implementations;

public class GenreService : IGenreService
{
    private readonly LibraryContext _context;

    public GenreService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO dto)
    {
        var newGenre = new Genre
        {
            Name = dto.Name,
            ParentGenreId = dto.ParentGenreId
        };

        _context.Genres.Add(newGenre);
        await _context.SaveChangesAsync();

        return new GenreResponseDTO(newGenre.Id, newGenre.Name, newGenre.ParentGenreId);
    }

    public async Task<GenreResponseDTO> GetGenreAsync(Guid id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
            throw new KeyNotFoundException("Genre not found");

        return new GenreResponseDTO(genre.Id, genre.Name, genre.ParentGenreId);
    }

    public async Task<PaginatedResponse<GenreResponseDTO>> GetGenresAsync(int page = 1, int pageSize = 20)
    {
        var genresQuery = _context.Genres
            .Select(g => new GenreResponseDTO(g.Id, g.Name, g.ParentGenreId))
            .AsNoTracking();

        return await PaginatedResponse<GenreResponseDTO>.CreateAsync(genresQuery, page, pageSize);
    }
}