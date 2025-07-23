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

internal sealed class NotificationService : INotificationService
{
    private readonly CoreDbContext _context;
    private readonly IMapper _mapper;
    public NotificationService(CoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CreateGeneralNotificationDto> CreateGeneralNotificationAsync(CreateGeneralNotificationDto notification)
    {
        notification.RecipientType = NotificationRecipientType.General;
        // Map the incoming DTO to the Notification entity model
        var entity = _mapper.Map<Notification>(notification);
        entity.State = EfState.Active;
        await _context.Notifications.AddAsync(entity);
        await _context.SaveChangesAsync();
        // Map the saved entity back to a DTO to return;
        return _mapper.Map<CreateGeneralNotificationDto>(entity);
    }

    public async Task<CreateUserNotificationDto> CreateNotificationForSpecificUserAsync(CreateUserNotificationDto notification)
    {
        notification.RecipientType = NotificationRecipientType.SpecificUser;
        // Map the incoming DTO to the Notification entity model
        var entity = _mapper.Map<Notification>(notification);
        entity.State = EfState.Active;
        await _context.Notifications.AddAsync(entity);
        await _context.SaveChangesAsync();
        // Map the saved entity back to a DTO to return;
        return _mapper.Map<CreateUserNotificationDto>(entity);
    }

    public Task CreateNotificationsForMultipleUsersAsync(CreateUserNotificationDto notification, List<string> userIds)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteNotificationAsync(Guid Id)
    {
        var entity = await _context.Notifications.FindAsync(Id) ?? throw new KeyNotFoundException($"Notificaiton with ID {Id} not found.");
        entity.State = EfState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task<NotificationDto?> FindNotificationByIdAsync(Guid id)
    {
        var entity = await _context.Notifications.FindAsync(id) ?? throw new KeyNotFoundException($"Notificaiton with ID {id} not found.");
        var notification = _mapper.Map<NotificationDto>(entity);
        return notification;
    }

    public async Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationForClientPagination(FilterRequest  filter)
    {
        var entityQuery = _context.Notifications
            .Where(m => m.State != EfState.Deleted)
            .OrderByDescending(m => m.CreatedOn)
            .AsNoTracking();
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entityQuery, filter);
        var data = _mapper.Map<List<NotificationListDto>>(entityResult.Datasource);
        return new PaginationResponse<NotificationListDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Datasource = data
        };
    }

    public async Task<PaginationResponse<NotificationListDto>> GetGeneralNotificationPagination(FilterRequest filter)
    {
        var entityQuery = _context.Notifications
            .Where(m => m.State != EfState.Deleted && m.NotificationRecipient == NotificationRecipientType.General)
            .OrderByDescending(m => m.CreatedOn)
            .AsNoTracking();
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entityQuery, filter);
        var data = _mapper.Map<List<NotificationListDto>>(entityResult.Datasource);
        return new PaginationResponse<NotificationListDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Datasource = data
        };
    }

    public async Task<string?> GetUserDeviceTokenByUserIdAsync(Guid id)
    {
        //Find user with the givin ID
        var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        //retrive FCM token from User record;
        var fcmToken = user.DeviceToken;
        return fcmToken;
    }

    public async Task<PaginationResponse<NotificationListDto>> GetUserNotificationPagination(FilterRequest  filter)
    {
        var entityQuery = _context.Notifications
           .Where(m => m.State != EfState.Deleted && m.NotificationRecipient == NotificationRecipientType.SpecificUser)
           .OrderByDescending(m => m.CreatedOn)
           .AsNoTracking();
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entityQuery, filter);
        var data = _mapper.Map<List<NotificationListDto>>(entityResult.Datasource);
        return new PaginationResponse<NotificationListDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Datasource = data
        };
    }

    public Task PermanentDeleteNotificationAsync(List<Guid> Ids)
    {
        throw new NotImplementedException();
    }

    public Task ResendNotificationAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<NotificationDto> UpdateNotificationAsync(NotificationDto notification)
    {
        throw new NotImplementedException();
    }
}