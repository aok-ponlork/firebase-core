using AutoMapper;
using Firebase_Auth.Common.Filters;
using Firebase_Auth.Context;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authorization;
using Firebase_Auth.Helper.Response;
using Firebase_Auth.Services.Authorization.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

internal sealed class RolePermissionService : IRolePermissionService
{
    private readonly CoreDbContext _context;
    private readonly FirebaseAuth _firebaseAuth;
    private readonly IMapper _mapper;

    public RolePermissionService(CoreDbContext context, FirebaseAuth firebaseAuth, IMapper mapper)
    {
        _context = context;
        _firebaseAuth = firebaseAuth;
        _mapper = mapper;
    }

    #region Role
    public async Task<PaginationResponse<RoleDto>> GetAllRolesAsync(FilterRequest  filter)
    {
        var entityQuery = _context.Roles.Where(m => m.State != EfState.Deleted).AsNoTracking();
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entityQuery, filter);
        var roles = _mapper.Map<List<RoleDto>>(entityResult.Data);
        return new PaginationResponse<RoleDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Data = roles
        };
    }

    public async Task<RoleDto> CreateRoleAsync(RoleCreateDto req)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == req.Name))
            throw new InvalidOperationException($"Role '{req.Name}' already exists");

        var role = _mapper.Map<Role>(req);
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }


    public async Task<RoleDto> UpdateRoleAsync(Guid id, string name, string description)
    {
        var role = await _context.Roles.FindAsync(id) ?? throw new KeyNotFoundException($"Role with ID {id} not found");

        // Check if new name already exists for another role
        if (await _context.Roles.AnyAsync(r => r.Name == name && r.Id != id))
            throw new InvalidOperationException($"Role '{name}' already exists");

        role.Name = name;
        role.Description = description;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        var result = _mapper.Map<RoleDto>(role);
        return result;
    }
    public async Task<RoleDto> GetRoleByIdAsync(Guid id)
    {
        var entity = await _context.Roles.FindAsync(id) ?? throw new KeyNotFoundException($"Role with ID {id} not found");
        var role = _mapper.Map<RoleDto>(entity);
        return role;
    }
    public async Task DeleteRoleAsync(Guid id)
    {
        var role = await _context.Roles.FindAsync(id) ?? throw new KeyNotFoundException($"Role with ID {id} not found");
        // Check if any users are using this role.
        // *NOTE: Currently using a one-to-one relationship (User → Role).
        // If switching to many-to-many later, use: 
        // await _context.UserRoles.AnyAsync(ur => ur.RoleId == id);
        var hasUsers = await _context.Users.AnyAsync(ur => ur.RoleId == id);
        if (hasUsers)
            throw new InvalidOperationException($"Cannot delete role '{role.Name}' because it is assigned to users");

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        // Get user
        var user = await _context.Users.FindAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found");

        // if same role already assigned
        if (user.RoleId == roleId)
            return;

        // Check if role exists
        var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);
        if (!roleExists)
            throw new KeyNotFoundException($"Role with ID {roleId} not found");

        // Assign role
        user.RoleId = roleId;
        await _context.SaveChangesAsync();

        // Sync claims
        await RefreshUserClaimsAsync(userId);
    }
    public async Task<string?> GetUserRoleAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Role!.Name)
            .FirstOrDefaultAsync();
    }
    #endregion


    #region  Role Permission
    public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId && u.Role != null)
            .SelectMany(u => u.Role!.RolePermissions.Select(rp => rp.Permission.Code))
            .Distinct()
            .ToListAsync();
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        // Check if role exists
        if (!await _context.Roles.AnyAsync(r => r.Id == roleId))
            throw new KeyNotFoundException($"Role with ID {roleId} not found");

        // Check if permission exists
        if (!await _context.Permissions.AnyAsync(p => p.Id == permissionId))
            throw new KeyNotFoundException($"Permission with ID {permissionId} not found");

        // Check if assignment already exists
        if (await _context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId))
            return; // Already assigned, nothing to do

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        await _context.RolePermissions.AddAsync(rolePermission);
        await _context.SaveChangesAsync();

        // update all users with this role
        var userIds = await _context.Users
            .Where(u => u.RoleId == roleId)
            .Select(u => u.Id)
            .ToListAsync();

        foreach (var userId in userIds)
        {
            await RefreshUserClaimsAsync(userId);
        }
    }

    public async Task<PermissionDto> CreatePermissionAsync(string name, string description, string code)
    {
        // Check if permission already exists
        if (await _context.Permissions.AnyAsync(p => p.Name == name || p.Code == code))
            throw new InvalidOperationException($"Permission '{name}' already exists");

        var permission = new Permission
        {
            Name = name,
            Description = description,
            Code = code,
            State = EfState.Active
        };

        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
        var model = _mapper.Map<PermissionDto>(permission);
        return model;
    }

    public async Task DeletePermissionAsync(Guid id)
    {
        var permission = await _context.Permissions.FindAsync(id)
            ?? throw new KeyNotFoundException($"Permission with ID {id} not found");

        var hasRoles = await _context.RolePermissions.AnyAsync(rp => rp.PermissionId == id);
        if (hasRoles)
            throw new InvalidOperationException(
                $"Cannot delete permission '{permission.Name ?? "Unknown"}' because it is assigned to roles");

        // soft delete
        permission.State = EfState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task<PaginationResponse<PermissionDto>> GetAllPermissionsAsync(FilterRequest  filter)
    {
        var entites = _context.Permissions.Where(p => p.State != EfState.Deleted);
        var entityResult = await PaginationHelper.CreatePaginatedResponse(entites, filter);
        var roles = _mapper.Map<List<PermissionDto>>(entityResult.Data);
        return new PaginationResponse<PermissionDto>
        {
            PageNumber = entityResult.PageNumber,
            PageSize = entityResult.PageSize,
            TotalRecords = entityResult.TotalRecords,
            Data = roles
        };

    }

    public async Task<PermissionDto> GetPermissionByIdAsync(Guid id)
    {
        var entity = await _context.Permissions.FindAsync(id) ?? throw new KeyNotFoundException($"Permission with ID {id} not found"); ;
        var permission = _mapper.Map<PermissionDto>(entity);
        return permission;
    }

    public async Task RefreshUserClaimsAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found");
        // Get user role
        var role = await GetUserRoleAsync(userId);

        // Get user permissions
        var permissions = await GetUserPermissionsAsync(userId);

        var claims = new Dictionary<string, object>();

        // Add role if not null or empty
        if (!string.IsNullOrWhiteSpace(role))
        {
            claims.Add("role", role);
        }

        // Add permissions
        if (permissions.Count != 0)
        {
            claims.Add("permissions", permissions);
        }

        await _firebaseAuth.SetCustomUserClaimsAsync(user.FirebaseUid, claims);
    }

    public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (rolePermission == null)
            return; // Not assigned, nothing to do

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();

        //Update user role perm
        var userIds = await _context.Users
            .Where(u => u.RoleId == roleId)
            .Select(u => u.Id)
            .ToListAsync();

        foreach (var userId in userIds)
        {
            await RefreshUserClaimsAsync(userId);
        }
    }

    public async Task RemoveRoleFromUserAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found");

        if (user.RoleId == null)
            return; // Already has no role

        user.RoleId = null;
        await _context.SaveChangesAsync();

        // Update user claims
        await RefreshUserClaimsAsync(userId);
    }

    public async Task<PermissionDto> UpdatePermissionAsync(Guid id, string name, string description)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            throw new KeyNotFoundException($"Permission with ID {id} not found");

        // Check if new name already exists for another permission
        if (await _context.Permissions.AnyAsync(p => p.Name == name && p.Id != id))
            throw new InvalidOperationException($"Permission '{name}' already exists");

        permission.Name = name;
        permission.Description = description;

        _context.Permissions.Update(permission);
        await _context.SaveChangesAsync();

        var model = _mapper.Map<PermissionDto>(permission);
        return model;
    }
    #endregion
    //Remove from database (Remove the soft remove data)
    public async Task PermanentDeleteRoleAsync(List<Guid> ids)
    {
        var roles = await _context.Roles
            .Where(r => ids.Contains(r.Id) && r.State == EfState.Deleted)
            .ToListAsync();

        if (!roles.Any())
            return;

        // Check if any role is assigned to users
        var assignedRoleIds = await _context.Users
            .Where(u => u.RoleId != null && ids.Contains(u.RoleId.Value))
            .Select(u => u.RoleId!.Value)
            .Distinct()
            .ToListAsync();

        if (assignedRoleIds.Any())
            throw new InvalidOperationException("Cannot delete roles that are currently assigned to users.");

        _context.Roles.RemoveRange(roles);
        await _context.SaveChangesAsync();
    }

    public async Task PermanentDeletePermissionAsync(List<Guid> ids)
    {
        var permissions = await _context.Permissions
           .Where(p => ids.Contains(p.Id) && p.State == EfState.Deleted)
            .ToListAsync();

        if (!permissions.Any())
            return;

        // Check if any permission is assigned to roles
        var assignedPermissionIds = await _context.RolePermissions
            .Where(rp => ids.Contains(rp.PermissionId))
            .Select(rp => rp.PermissionId)
            .Distinct()
            .ToListAsync();

        if (assignedPermissionIds.Any())
            throw new InvalidOperationException("Cannot delete permissions that are currently assigned to roles.");

        _context.Permissions.RemoveRange(permissions);
        await _context.SaveChangesAsync();
    }

}