using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Models.Movies;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Firebase_Auth.Controllers.Movies;

[Route("api/movies")]
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
    public async Task<IActionResult> ListMovieAsync([FromQuery] FilterRequest filter)
    {

        try
        {
            Console.WriteLine($"Filters count: {filter.Filters?.Count ?? 0}");
            Console.WriteLine($"SortBy: {filter.SortBy}");
            Console.WriteLine($"Filters JSON: {JsonConvert.SerializeObject(filter.Filters)}");
            var movies = await _movieService.ListMovieAsync(filter);
            return ToSuccess("Success get movies list!", movies);
        }
        catch (Exception ex)
        {
            return ToInternalServerError($"An unexpected error occurred: {ex.Message}");
        }
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