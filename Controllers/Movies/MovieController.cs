using Firebase_Auth.Common;
using Firebase_Auth.Data.Models.Movies;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Movies;
[Route("api/movie")]
[Authorize]
public class MovieController : CoreController
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService, ILogger<MovieController> logger) : base(logger)
    {
        _movieService = movieService;
    }
    [AllowAnonymous]
    [HttpGet("list-movie")]
    public async Task<IActionResult> ListMovieAsync([FromQuery] PaginationFilter filter)
    {
        var movies = await _movieService.ListMovieAsync(filter);
        return ToSuccess("Success get movies list!", movies);
    }
    [Authorize(Roles = "admin")]
    [HttpPost("movie")]
    public async Task<IActionResult> CreateMovieAsync([FromBody] MovieCreateDto req)
    {
        try
        {
            var movies = await _movieService.CreateMovieAsync(req);
            return ToSuccess("Success get movies list!", movies);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
}