public record GenreResponseDTO(
    Guid Id,
    string Name,
    Guid? ParentGenreId,
    string? ParentGenreName,
    IEnumerable<GenreSubGenreDTO>? SubGenres
);

public record GenreSubGenreDTO(Guid Id, string Name);  // DTO для поджанров