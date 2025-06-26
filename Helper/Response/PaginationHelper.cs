using Firebase_Auth.Common.Filters;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Helper.Response;

public static class PaginationHelper
{
    public static async Task<PaginationResponse<T>> CreatePaginatedResponse<T>(IQueryable<T> query, FilterRequest filter) where T : class
    {
        var combinedFilter = BuildNestedFilter(filter.Filters);
        query = query.ApplyFilters(combinedFilter);

        // sorting
        query = query.ApplySort(filter.SortBy ?? "", filter.SortDirection);

        var totalRecords = await query.CountAsync();

        // pagination
        var data = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize).OrderDescending()
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = filter.PageIndex,
            PageSize = filter.PageSize,
            TotalRecords = totalRecords,
            Data = data
        };
    }
    static DynamicFilter BuildNestedFilter(List<DynamicFilter> filters)
    {
        if (filters == null || filters.Count == 0)
            return new DynamicFilter { Logic = "AND", Filters = new List<DynamicFilter>() };
        // Start from the last filter
        DynamicFilter nestedFilter = filters.Last();
        // Walk backward, nesting each previous filter with the next, using that filter's logic or default AND
        for (int i = filters.Count - 2; i >= 0; i--)
        {
            nestedFilter = new DynamicFilter
            {
                Logic = filters[i].Logic?.ToUpper() ?? "AND",
                Filters = new List<DynamicFilter> { filters[i], nestedFilter }
            };
        }
        return nestedFilter;
    }
}
