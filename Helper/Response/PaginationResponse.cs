namespace Firebase_Auth.Helper.Response;

public class PaginationResponse<T> where T : class
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => PageSize > 0
         ? Math.Max(1, (int)Math.Ceiling((double)TotalRecords / PageSize))
         : 1;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public List<T> Datasource { get; set; } = [];
}

