using Microsoft.EntityFrameworkCore;

namespace BookShop.Shared.DTO.Response;

public class PaginatedResponse<TResult>
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public IEnumerable<TResult> Data { get; }

    public PaginatedResponse(IEnumerable<TResult> data, int totalCount, int page, int pageSize)
    {
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Data = data;
    }

    public static async Task<PaginatedResponse<TResult>> CreateAsync(IQueryable<TResult> source, int page = 1, int pageSize = 20)
    {
        int totalCount = await source.CountAsync();
        var data = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResponse<TResult>(data, totalCount, page, pageSize);
    }
}