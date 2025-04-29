using AutoMapper;
using Firebase_Auth.Data.Entities.Movies;
using Firebase_Auth.Data.Models.Movies;



namespace Firebase_Auth.Data.Profiles;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        // From entity to DTO
        CreateMap<Movie, MovieGetDto>();
        CreateMap<Movie, MovieListDto>();

        // From DTO to entity (for creating and updating)
        CreateMap<MovieCreateDto, Movie>();
        CreateMap<MovieUpdateDto, Movie>();
    }
}
