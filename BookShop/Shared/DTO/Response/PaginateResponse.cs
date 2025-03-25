using Microsoft.EntityFrameworkCore;

namespace BookShop.Shared.DTO.Response;

public class PaginatedResponse<TResult>
{
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }
    public IEnumerable<TResult> Data { get; private set; }
    
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PaginatedResponse(IEnumerable<TResult> data, int totalCount, int page, int pageSize)
    {
        Page = Math.Max(page, 1); // Минимальное значение 1
        PageSize = Math.Max(pageSize, 1); // Минимальное значение 1
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        Data = data;
    }

    public static async Task<PaginatedResponse<TResult>> CreateAsync(IQueryable<TResult> source, int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Max(pageSize, 1);

        int totalCount = await source.CountAsync();
        var data = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<TResult>(data, totalCount, page, pageSize);
    }
}