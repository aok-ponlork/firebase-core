using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Common.Filters;

public class PaginationFilter
{
    [FromQuery(Name = "pageIndex")]
    public int PageIndex { get; set; } = 1;

    [FromQuery(Name = "pageSize")]
    public int PageSize { get; set; } = 25;
}