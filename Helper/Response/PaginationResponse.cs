namespace Firebase_Auth.Helper.Response;

public class PaginationResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
    public List<T> Datasource { get; set; } = new();
}
