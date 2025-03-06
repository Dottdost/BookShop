namespace BookShop.Shared.DTO.Requests;

public record CreateGenreDTO(string Name, Guid? ParentGenreId = null);