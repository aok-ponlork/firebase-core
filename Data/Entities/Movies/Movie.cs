namespace Firebase_Auth.Data.Entities.Movies;

public class Movie : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int DurationMinutes { get; set; } // Total duration in minutes
    public string? Language { get; set; }
    public string? Country { get; set; }
    public double Rating { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public List<string> Cast { get; set; } = [];
    public List<string> Directors { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; } = string.Empty;
}

// public class Actor
// {
//     public int Id { get; set; }
//     public string FullName { get; set; } = null!;
//     public string? Bio { get; set; }
//     public string? PhotoUrl { get; set; }
//     public List<Movie> Movies { get; set; } = [];
// }

// public class Director
// {
//     public int Id { get; set; }
//     public string FullName { get; set; } = null!;
//     public string? Bio { get; set; }
//     public string? PhotoUrl { get; set; }
//     public List<Movie> Movies { get; set; } = [];
// }

// public class Genre
// {
//     public int Id { get; set; }
//     public string Name { get; set; } = string.Empty;
//     public List<Movie> Movies { get; set; } = [];
// }