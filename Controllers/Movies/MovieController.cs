using Firebase_Auth.Common;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Movies;
[Route("api/movie")]
public class MovieController : CoreController
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService, ILogger<MovieController> logger) : base(logger)
    {
        _movieService = movieService;
    }
    [Authorize(Roles = "user")]
    [HttpGet("list-movie")]
    public async Task<IActionResult> ListMovieAsync([FromQuery] PaginationFilter filter)
    {
        var movies = await _movieService.ListMovieAsync(filter);
        return ToSuccess("Success get movies list!", movies);
    }
}