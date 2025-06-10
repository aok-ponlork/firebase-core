
using AutoMapper;
using Firebase_Auth.Common.Filters;
using Firebase_Auth.Context;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Models.Common.Notification;
using Firebase_Auth.Helper.Response;
using Firebase_Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Services;
internal sealed class NotificationTopicService : INotificationTopicService
{
    private readonly CoreDbContext _context;
    private readonly IMapper _mapper;
    public NotificationTopicService(CoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NotificationTopicDto> AddAsync(CreateNotificationTopicDto createDto)
    {
        var entity = _mapper.Map<NotificationTopic>(createDto);
        entity.State = EfState.Active;
        _context.Topics.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<NotificationTopicDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var record = await _context.Topics.FindAsync(id) ?? throw new KeyNotFoundException("Not found");
        record.State = EfState.Deleted;
        await _context.SaveChangesAsync();
    }
    
    public async Task<NotificationTopicDto> GetByIdAsync(Guid id)
    {
        var record = await _context.Topics.FindAsync(id) ?? throw new KeyNotFoundException("Not found!");
        return _mapper.Map<NotificationTopicDto>(record);
    }

    public async Task<PaginationResponse<NotificationTopicDto>> List(FilterRequest filter)
    {
        var entityQuery = _context.Topics
            .Where(m => m.State != EfState.Deleted)
            .OrderByDescending(m => m.CreatedOn)
            .AsNoTracking();
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entityQuery, filter);
        var data = _mapper.Map<List<NotificationTopicDto>>(entityResult.Data);
        return new PaginationResponse<NotificationTopicDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Data = data
        };
    }

    public async Task<NotificationTopicDto> UpdateAsync(Guid id, CreateNotificationTopicDto updateDto)
    {
        var record = await _context.Topics.FindAsync(id)
                     ?? throw new KeyNotFoundException("Notification topic not found");
        record.TopicName = updateDto.TopicName;
        record.Description = updateDto.Description;
        record.UpdatedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        // TODO: Handle resubscribing users if topic name changes or update impacts subscriptions
        return _mapper.Map<NotificationTopicDto>(record);
    }

}