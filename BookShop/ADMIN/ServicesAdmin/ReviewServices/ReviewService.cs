using BookShop.ADMIN.DTOs;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ADMIN.ServicesAdmin.ReviewServices;

public class ReviewService : IReviewService
{
    private readonly LibraryContext _context;

    public ReviewService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<ReviewDto>> GetAllAsync()
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Book)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                UserName = r.User.UserName,
                BookTitle = r.Book.Title,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }
}
