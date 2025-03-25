namespace BookShop.Shared.DTO.Requests;

public record CreateBookDTO(
    string Title,
    string Author,
    decimal Price,
    int Stock,
    string Description,
    string ImageUrl,
    Guid GenreId,
    Guid PublisherId
);