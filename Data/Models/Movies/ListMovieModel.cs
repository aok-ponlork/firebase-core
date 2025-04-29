namespace Firebase_Auth.Data.Models.Movies;
public class MovieListDto : BaseModel
{
    public required string Title { get; set; }
    public double Rating { get; set; }
    public string? PosterUrl { get; set; }
}
