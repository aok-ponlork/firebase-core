using Firebase_Auth.Common;
using Firebase_Auth.Data.Models.Movies;

namespace Firebase_Auth.Services.Interfaces;
public interface IMovieService
{
    Task<MovieGetDto> GetMovieByIdAsync(Guid id);
    Task<PaginationResponse<MovieListDto>> ListMovieAsync(PaginationFilter filter);
    Task<MovieCreateDto> CreateMovieAsync(MovieCreateDto dto);
    Task<MovieUpdateDto> UpdateMovieAsync(Guid id, MovieUpdateDto dto);
    Task DeleteMovieByIdAsync(Guid id);
    Task PermanentDeleteMovieAsync(List<Guid> Ids);

}
