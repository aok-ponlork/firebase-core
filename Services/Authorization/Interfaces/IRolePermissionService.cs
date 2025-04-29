using Firebase_Auth.Common;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authorization;

namespace Firebase_Auth.Services.Authorization.Interfaces;
public interface IRolePermissionService
{
    Task<PaginationResponse<RoleDto>> GetAllRolesAsync(PaginationFilter filter);
    Task<RoleDto> GetRoleByIdAsync(Guid id);
    Task<RoleDto> CreateRoleAsync(RoleCreateDto role);
    Task<RoleDto> UpdateRoleAsync(Guid id, string name, string description);
    Task DeleteRoleAsync(Guid id);
    Task PermanentDeleteRoleAsync(List<Guid> Ids);

    Task<PaginationResponse<PermissionDto>> GetAllPermissionsAsync(PaginationFilter filter);
    Task<PermissionDto> GetPermissionByIdAsync(Guid id);
    Task<PermissionDto> CreatePermissionAsync(string name, string description, string code);
    Task<PermissionDto> UpdatePermissionAsync(Guid id, string name, string description);
    Task DeletePermissionAsync(Guid id);
    Task PermanentDeletePermissionAsync(List<Guid> Ids);

    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
    Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);

    Task AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task RemoveRoleFromUserAsync(Guid userId);

    Task<List<string>> GetUserPermissionsAsync(Guid userId);
    Task<string?> GetUserRoleAsync(Guid userId);

    Task RefreshUserClaimsAsync(Guid userId);
}