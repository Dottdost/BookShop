public record GenreResponseDTO(
    int Id,  
    string Name,
    int? ParentGenreId,  
    string? ParentGenreName,
    IEnumerable<GenreSubGenreDTO>? SubGenres
);

public record GenreSubGenreDTO(int Id, string Name);  