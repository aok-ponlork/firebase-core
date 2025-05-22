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
    [HttpGet]
    public async Task<IActionResult> ListMovieAsync([FromQuery] PaginationFilter filter)
    {
        var movies = await _movieService.ListMovieAsync(filter);
        return ToSuccess("Success get movies list!", movies);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieByIdAsync(string id)
    {
        try
        {
            var result = await _movieService.GetMovieByIdAsync(Guid.Parse(id));
            return ToSuccess("Success get movie by Id.", result);
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpPost]
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
    [HttpPut]
    public async Task<IActionResult> UpdateMovieAsync([FromQuery] string id, [FromBody] MovieUpdateDto req)
    {
        try
        {
            var movies = await _movieService.UpdateMovieAsync(Guid.Parse(id), req);
            return ToSuccess("Success update!", movies);
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieByIdAsync(string id)
    {
        try
        {
            await _movieService.DeleteMovieByIdAsync(Guid.Parse(id));
            return ToNoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
    }
}