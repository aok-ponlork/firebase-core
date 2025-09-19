using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Common.Filters;

public class DynamicFilter
{
    public string? Field { get; set; }
    public string? Operator { get; set; }
    public object? Value { get; set; }
    public string? Logic { get; set; } = "AND";
    public List<DynamicFilter>? Filters { get; set; }
    public bool HasNestedFilters => Filters?.Count > 0;
    public bool IsValidFilter => !string.IsNullOrEmpty(Field) && !string.IsNullOrEmpty(Operator);
}

public class SimpleFilter : PaginationFilter
{
    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }

    [FromQuery(Name = "sortDirection")]
    public string SortDirection { get; set; } = "ASC";

    [FromQuery(Name = "search")]
    public string? Search { get; set; }
}

public class FilterRequest : PaginationFilter
{
    [FromQuery(Name = "filters")]
    public List<DynamicFilter> Filters { get; set; } = new();
    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }

    [FromQuery(Name = "sortDirection")]
    public string SortDirection { get; set; } = "ASC";

    [FromQuery(Name = "search")]
    public string? Search { get; set; }

}

public class PaginationFilter
{
    private const int DefaultPageSize = 25;
    private const int MaxPageSize = 100;

    private int _pageIndex = 1;
    private int _pageSize = DefaultPageSize;

    [FromQuery(Name = "pageIndex")]
    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    [FromQuery(Name = "pageSize")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value < 1 || value > MaxPageSize) ? DefaultPageSize : value;
    }
}
