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
    public class UserPhoneConfiguration : IEntityTypeConfiguration<UserPhone>
    {
        public void Configure(EntityTypeBuilder<UserPhone> builder)
        {
            builder.ToTable("UserPhones");
            builder.HasKey(p => p.PhoneId);
            builder.Property(p => p.PhoneId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();
            builder.HasIndex(p => p.PhoneNumber).IsUnique();
            builder.Property(p => p.IsVerified)
                .HasDefaultValue(false);
            builder.Property(p => p.IsPrimary)
                .HasDefaultValue(false);

            builder.HasOne(p => p.User)
                .WithMany(u => u.UserPhones)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
