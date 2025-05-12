using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Entities.Common.Notification;
using Firebase_Auth.Data.Entities.Movies;
using Firebase_Auth.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Context
{
    public class CoreDbContext : DbContext
    {
        //User and role permission.
        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<Role> Roles => Set<Role>();
        public virtual DbSet<Permission> Permissions => Set<Permission>();
        public virtual DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public virtual DbSet<Notification> Notifications => Set<Notification>();
        //Other entites
        public virtual DbSet<Movie> Movies => Set<Movie>();
        public CoreDbContext(DbContextOptions contextOption) : base(contextOption)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User & Role permission.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.FirebaseUid)
                .IsUnique();

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            //Other entites.

            //Seed data
            modelBuilder.SeedMovies();
            modelBuilder.SeedRolesAndPermissions();
        }
    }
}