namespace Shared.DTO.Response;

public record BookResponseDTO(
    Guid Id,
    string Title,
    string Author,
    decimal Price,
    int Stock,
    string Description,
    string ImageUrl,
    string Genre,
    string Publisher
);