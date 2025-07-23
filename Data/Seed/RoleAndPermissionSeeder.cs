using Firebase_Auth.Data.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Data.Seed
{
    public static class RolePermissionSeeder
    {
        public static void SeedRolesAndPermissions(this ModelBuilder modelBuilder)
        {
            // Adding Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "admin", Description = "Administrator role with full permissions" },
                new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "moderator", Description = "Editor role with editing permissions" },
                new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "user", Description = "User role with viewing permissions" },
                new Role { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "root", Description = "Root" }
            );

            // Adding Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "CreateContent", Description = "Permission to create content", Code = "CreateContent" },
                new Permission { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "EditContent", Description = "Permission to edit content", Code = "EditContent" },
                new Permission { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "DeleteContent", Description = "Permission to delete content", Code = "DeleteContent" },
                new Permission { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "ViewContent", Description = "Permission to view content", Code = "ViewContent" }
            );

            // Assigning Permissions to Roles
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), PermissionId = Guid.Parse("44444444-4444-4444-4444-444444444444") }, // Root -> CreateContent
                new RolePermission { RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), PermissionId = Guid.Parse("55555555-5555-5555-5555-555555555555") }, // Root -> EditContent
                new RolePermission { RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), PermissionId = Guid.Parse("66666666-6666-6666-6666-666666666666") }, // Root -> DeleteContent
                new RolePermission { RoleId = Guid.Parse("99999999-9999-9999-9999-999999999999"), PermissionId = Guid.Parse("77777777-7777-7777-7777-777777777777") }, // Root -> ViewContent

                new RolePermission { RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PermissionId = Guid.Parse("44444444-4444-4444-4444-444444444444") }, // Admin -> CreateContent
                new RolePermission { RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PermissionId = Guid.Parse("55555555-5555-5555-5555-555555555555") }, // Admin -> EditContent
                new RolePermission { RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PermissionId = Guid.Parse("66666666-6666-6666-6666-666666666666") }, // Admin -> DeleteContent
                new RolePermission { RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PermissionId = Guid.Parse("77777777-7777-7777-7777-777777777777") }, // Admin -> ViewContent

                new RolePermission { RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), PermissionId = Guid.Parse("55555555-5555-5555-5555-555555555555") }, // Editor -> EditContent
                new RolePermission { RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222"), PermissionId = Guid.Parse("77777777-7777-7777-7777-777777777777") }, // Editor -> ViewContent

                new RolePermission { RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), PermissionId = Guid.Parse("77777777-7777-7777-7777-777777777777") }  // User -> ViewContent
            );
        }
    }
}
