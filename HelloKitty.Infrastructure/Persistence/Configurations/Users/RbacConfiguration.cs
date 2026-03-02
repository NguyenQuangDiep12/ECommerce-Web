using HelloKitty.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Users
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(u => u.RoleId);

            builder.Property(u => u.RoleName)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(u => u.RoleName).IsUnique();

            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(u => u.UserRoles)
                .WithOne(c => c.Role)
                .HasForeignKey(i => i.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.RolePermissions)
                .WithOne(c => c.Role)
                .HasForeignKey(i => i.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Role { RoleName = "Admin", Description = "Quản trị viên hệ thống" },
                new Role { RoleName = "Customer", Description = "Khách hàng" },
                new Role { RoleName = "Staff", Description = "Nhân viên" }
            );
        }

        public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
        {
            public void Configure(EntityTypeBuilder<UserRole> builder)
            {
                builder.ToTable("UserRoles");
                // Composition Key
                builder.HasKey(u => new { u.UserId, u.RoleId });
                builder.Property(u => u.AssignedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }

        public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
        {
            public void Configure(EntityTypeBuilder<Permission> builder)
            {
                builder.ToTable("Permissions");
                builder.HasKey(u => u.PermissionId);

                builder.Property(u => u.PermissionName)
                    .HasMaxLength(100)
                    .IsRequired();
                builder.HasIndex(u => u.PermissionName)
                    .IsUnique();
                builder.Property(u => u.Description)
                    .HasMaxLength(255);
                builder.Property(u => u.Group)
                    .HasConversion<int>();

                builder.HasMany(u => u.RolePermissions)
                    .WithOne(s => s.Permission)
                    .HasForeignKey(o => o.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
            }
        }
        public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
        {
            public void Configure(EntityTypeBuilder<RolePermission> builder)
            {
                builder.ToTable("RolePermissions");
                builder.HasKey(u => new { u.RoleId , u.PermissionId });

                builder.Property(u => u.AssignedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
    }
}
