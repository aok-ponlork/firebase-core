namespace Firebase_Auth.Services.Interfaces.Base;
public interface IBaseService<TCreateDto, TEntity, TDto>
    where TEntity : class
{
    Task<TDto> AddAsync(TCreateDto createDto);
    Task<TDto> GetByIdAsync(Guid id);
    Task<TDto> UpdateAsync(Guid id, TCreateDto updateDto);
    Task DeleteAsync(Guid id);
}

