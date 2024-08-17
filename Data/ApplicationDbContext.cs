
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Models;
using System.Linq;
using System.Reflection.Emit;

namespace PhotoGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            base.OnModelCreating(builder);

            // Изменение конфигурации для SQLite
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    builder.Entity(entityType.Name).Property(property.Name).HasColumnType("TEXT");
                }
            }

            // Конфигурация Identity для SQLite
            builder.Entity<IdentityUser>(entity =>
            {
                entity.Property(m => m.Id).HasColumnType("TEXT");
                entity.Property(m => m.UserName).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.NormalizedUserName).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.Email).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.NormalizedEmail).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.PasswordHash).HasColumnType("TEXT");
                entity.Property(m => m.SecurityStamp).HasColumnType("TEXT");
                entity.Property(m => m.ConcurrencyStamp).HasColumnType("TEXT");
                entity.Property(m => m.PhoneNumber).HasColumnType("TEXT");
                entity.Property(m => m.LockoutEnd).HasColumnType("TEXT");
                entity.Property(m => m.LockoutEnabled).HasColumnType("INTEGER");
                entity.Property(m => m.AccessFailedCount).HasColumnType("INTEGER");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(m => m.Id).HasColumnType("TEXT");
                entity.Property(m => m.Name).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.NormalizedName).HasColumnType("TEXT").HasMaxLength(256);
                entity.Property(m => m.ConcurrencyStamp).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasColumnType("TEXT");
                entity.Property(m => m.RoleId).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasColumnType("TEXT").HasMaxLength(128);
                entity.Property(m => m.ProviderKey).HasColumnType("TEXT").HasMaxLength(128);
                entity.Property(m => m.ProviderDisplayName).HasColumnType("TEXT");
                entity.Property(m => m.UserId).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasColumnType("TEXT");
                entity.Property(m => m.LoginProvider).HasColumnType("TEXT").HasMaxLength(128);
                entity.Property(m => m.Name).HasColumnType("TEXT").HasMaxLength(128);
                entity.Property(m => m.Value).HasColumnType("TEXT");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(m => m.Id).HasColumnType("INTEGER");
                entity.Property(m => m.UserId).HasColumnType("TEXT");
                entity.Property(m => m.ClaimType).HasColumnType("TEXT");
                entity.Property(m => m.ClaimValue).HasColumnType("TEXT");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.Property(m => m.Id).HasColumnType("INTEGER");
                entity.Property(m => m.RoleId).HasColumnType("TEXT");
                entity.Property(m => m.ClaimType).HasColumnType("TEXT");
                entity.Property(m => m.ClaimValue).HasColumnType("TEXT");
            });
        }
    }
}