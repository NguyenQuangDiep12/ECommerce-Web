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
}
