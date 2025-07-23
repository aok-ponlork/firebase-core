namespace Firebase_Auth.Data.Models.Movies;
public class MovieUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public double? Rating { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public List<string>? Cast { get; set; }
    public List<string>? Directors { get; set; }
}
