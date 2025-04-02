using BookShop.ADMIN.DTOs;

namespace BookShop.ADMIN.ServicesAdmin.ReviewServices;

using BookShop.ADMIN.DTOs.DTOAdmin;

public interface IReviewService
{
    Task<List<ReviewDto>> GetAllAsync();
    Task<bool> DeleteAsync(Guid id);
}
