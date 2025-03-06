namespace BookShop.Shared.DTO.Requests;

public record GenreResponseDTO(
    Guid Id,
    string Name,
    Guid? ParentGenreId
);