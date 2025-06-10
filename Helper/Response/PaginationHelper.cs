using Firebase_Auth.Common.Filters;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Helper.Response;

public static class PaginationHelper
{
    public static async Task<PaginationResponse<T>> CreatePaginatedResponse<T>(IQueryable<T> query, FilterRequest filter) where T : class
    {
        var combinedFilter = new DynamicFilter
        {
            Filters = filter.Filters
        };

        query = query.ApplyFilters(combinedFilter);

        // sorting
        query = query.ApplySort(filter.SortBy ?? "", filter.SortDirection);

        var totalRecords = await query.CountAsync();

        // pagination
        var data = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = filter.PageIndex,
            PageSize = filter.PageSize,
            TotalRecords = totalRecords,
            Data = data
        };
    }

}
