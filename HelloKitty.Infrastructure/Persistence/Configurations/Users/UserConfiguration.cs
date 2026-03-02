using HelloKitty.Domain.Users.Entities;
using HelloKitty.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Gender).HasMaxLength(10);
            builder.Property(u => u.AvatarUrl).HasMaxLength(500);

            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.Status)
                .HasConversion<int>()
                .HasDefaultValue(UserStatus.Active);

            // Entity Relation

            // 1 - 1
            builder.HasOne(u => u.UserSensitve)
                .WithOne(s => s.User)
                .HasForeignKey<UserSensitve>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.UserCredential)
                .WithOne(s => s.User)
                .HasForeignKey<UserCredential>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 - N

            builder.HasMany(u => u.OAuths)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserWallets)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.VoucherUsages)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reviews)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.UserRoles)
                .WithOne(s => s.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
