using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Models.Movies;
using Firebase_Auth.Helper.Response;

namespace Firebase_Auth.Services.Interfaces;
public interface IMovieService
{
    Task<MovieGetDto> GetMovieByIdAsync(Guid id);
    Task<PaginationResponse<MovieListDto>> ListMovieAsync(FilterRequest filter);
    Task<MovieCreateDto> CreateMovieAsync(MovieCreateDto dto);
    Task<MovieUpdateDto> UpdateMovieAsync(Guid id, MovieUpdateDto dto);
    Task DeleteMovieByIdAsync(Guid id);
    Task PermanentDeleteMovieAsync(List<Guid> Ids);

}
