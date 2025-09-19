using Firebase_Auth.Common.Extensions;
using Firebase_Auth.Common.Filters;
using Firebase_Auth.Helper.Response;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Common.Helpers;

public static class PaginationHelper
{

    //pagination with Dynamic filter
    public static async Task<PaginationResponse<T>> CreatePaginatedResponseWithDynamicFilterAsync<T>(
        IQueryable<T> query,
        FilterRequest filterRequest,
        params string[] searchableFields) where T : class
    {
        // Apply search if specified
        if (!string.IsNullOrWhiteSpace(filterRequest.Search))
        {
            query = query.ApplySearch(filterRequest.Search, searchableFields);
        }

        // Apply filters
        var combinedFilter = BuildCombinedFilter(filterRequest.Filters);
        query = query.ApplyFilters(combinedFilter);

        // Apply sorting with fallback to Id
        query = query.ApplySort(filterRequest.SortBy ?? "Id", filterRequest.SortDirection);

        // Get total count before pagination
        var totalRecords = await query.CountAsync();

        // Apply pagination
        var data = await query
            .Skip((filterRequest.PageIndex - 1) * filterRequest.PageSize)
            .Take(filterRequest.PageSize)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = filterRequest.PageIndex,
            PageSize = filterRequest.PageSize,
            TotalRecords = totalRecords,
            Datasource = data
        };
    }

    public static async Task<PaginationResponse<T>> CreatePaginatedResponseAsync<T>(
           IQueryable<T> query,
           SimpleFilter filterRequest,
           params string[] searchableFields) where T : class
    {
        // Apply search if specified
        if (!string.IsNullOrWhiteSpace(filterRequest.Search))
        {
            query = query.ApplySearch(filterRequest.Search, searchableFields);
        }

        // Apply sorting with fallback to Id
        query = query.ApplySort(filterRequest.SortBy ?? "Id", filterRequest.SortDirection);

        // Get total count before pagination
        var totalRecords = await query.CountAsync();

        // Apply pagination
        var data = await query
            .Skip((filterRequest.PageIndex - 1) * filterRequest.PageSize)
            .Take(filterRequest.PageSize)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = filterRequest.PageIndex,
            PageSize = filterRequest.PageSize,
            TotalRecords = totalRecords,
            Datasource = data
        };
    }

    private static DynamicFilter? BuildCombinedFilter(List<DynamicFilter> filters)
    {
        if (filters == null || filters.Count == 0)
            return null;

        if (filters.Count == 1)
            return filters[0];

        return new DynamicFilter
        {
            Logic = "AND",
            Filters = filters
        };
    }
}