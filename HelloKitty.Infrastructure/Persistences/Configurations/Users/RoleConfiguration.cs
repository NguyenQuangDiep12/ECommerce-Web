using HelloKitty.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Users
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
    }
}