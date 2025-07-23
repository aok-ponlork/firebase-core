namespace Firebase_Auth.Data.Seed;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Firebase_Auth.Data.Entities.Movies;
using Firebase_Auth.Data.Constant;

public static class MovieSeeder
{
    public static List<Movie> GetMovieSeed()
    {
        // Use Bogus to generate fake movie data without dynamic values
        var faker = new Faker<Movie>()
            .RuleFor(m => m.Title, f => f.Commerce.ProductName())
            .RuleFor(m => m.Description, f => f.Lorem.Sentence())
            .RuleFor(m => m.ReleaseDate, f => f.Date.Past(10))
            .RuleFor(m => m.DurationMinutes, f => f.Random.Int(60, 180))
            .RuleFor(m => m.Language, f => f.Locale)
            .RuleFor(m => m.Country, f => f.Address.Country())
            .RuleFor(m => m.Rating, f => f.Random.Double(1.0, 10.0))
            .RuleFor(m => m.PosterUrl, f => f.Image.PicsumUrl())
            .RuleFor(m => m.TrailerUrl, f => f.Internet.Url())
            .RuleFor(m => m.Cast, f => f.Lorem.Words(f.Random.Int(3, 7)).ToList())
            .RuleFor(m => m.Directors, f => f.Lorem.Words(f.Random.Int(1, 3)).ToList())
            .RuleFor(m => m.State, f => EfState.Active);

        // Generate 50 fake movies
        var movies = faker.Generate(50);

        // Generate static GUIDs and assign them
        foreach (var movie in movies)
        {
            movie.Id = Guid.NewGuid(); // Static GUID here
        }

        return movies;
    }

    public static void SeedMovies(this ModelBuilder modelBuilder)
    {
        // Get the generated movie data
        var movies = GetMovieSeed();

        // Add static data to the ModelBuilder
        modelBuilder.Entity<Movie>().HasData(movies); // Add the list of movies
    }
}
