using AutoMapper;
using Firebase_Auth.Common.Filters;
using Firebase_Auth.Common.Helpers;
using Firebase_Auth.Context;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Entities.Movies;
using Firebase_Auth.Data.Models.Movies;
using Firebase_Auth.Helper.Response;
using Firebase_Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Services;

internal sealed class MovieService(CoreDbContext context, IMapper mapper) : IMovieService
{
    private readonly CoreDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    public async Task<MovieCreateDto> CreateMovieAsync(MovieCreateDto movie)
    {
        try
        {
            var entity = _mapper.Map<Movie>(movie);
            entity.State = EfState.Active;
            await _context.Movies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<MovieCreateDto>(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteMovieByIdAsync(Guid id)
    {
        var result = await _context.Movies.FindAsync(id) ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");
        // Mark as deleted for soft delete.
        result.State = EfState.Deleted;
        await _context.SaveChangesAsync();
    }


    public async Task<MovieGetDto> GetMovieByIdAsync(Guid id)
    {
        var entity = await _context.Movies.FindAsync(id) ?? throw new KeyNotFoundException($"Movie with ID {id} not found.");
        var result = _mapper.Map<MovieGetDto>(entity);
        return result;
    }

    public async Task<PaginationResponse<MovieListDto>> ListMovieAsync(SimpleFilter filter)
    {
        // based query
        var entityQuery = _context.Movies
            .Where(m => m.State != EfState.Deleted)
            .AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            entityQuery = entityQuery.Where(m =>
                EF.Functions.ToTsVector("english", m.Title + " " + m.Description)
                .Matches(EF.Functions.WebSearchToTsQuery("english", filter.Search))
            );
        }
        // Use the helper to handle pagination of entities
        var entityResult = await PaginationHelper.CreatePaginatedResponseAsync(entityQuery, filter);
        //Map data
        var data = _mapper.Map<List<MovieListDto>>(entityResult.Datasource);
        return new PaginationResponse<MovieListDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Datasource = data
        };
    }

    public async Task PermanentDeleteMovieAsync(List<Guid> Ids)
    {
        try
        {
            if (Ids.Count == 1)
            {
                // For a single ID find and remove it directly
                var movie = await _context.Movies.FindAsync(Ids[0]);
                if (movie != null)
                {
                    _context.Movies.Remove(movie);
                }
            }
            else
            {
                // For multiple IDs query and remove them as a batch
                var moviesToDelete = await _context.Movies
                    .Where(m => Ids.Contains(m.Id))
                    .ToListAsync();
                if (moviesToDelete.Count != 0)
                {
                    _context.Movies.RemoveRange(moviesToDelete);
                }
            }
            // Save changes
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to permanently delete movies. See inner exception for details.", ex);
        }
    }

    public async Task<MovieUpdateDto> UpdateMovieAsync(Guid id, MovieUpdateDto dto)
    {
        try
        {
            var entity = await _context.Movies.FindAsync(id) ??
                throw new KeyNotFoundException($"Movie with ID {id} not found.");
            entity.State = EfState.Modified;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<MovieUpdateDto>(entity);
            return result;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to update movie with ID {id}.", ex);
        }
    }
}