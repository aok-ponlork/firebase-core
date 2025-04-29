using Firebase_Auth.Common;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Helper.Response;
public static class PaginationHelper
{
    public static async Task<PaginationResponse<T>> CreatePaginatedResponse<T>(IQueryable<T> query, PaginationFilter filter) where T : class
    {
        var totalRecords = await query.CountAsync();

        var data = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalRecords = totalRecords,
            Data = data
        };
    }
}
