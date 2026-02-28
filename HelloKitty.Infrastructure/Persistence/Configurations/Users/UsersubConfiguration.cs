using HelloKitty.API.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Users
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");
            builder.HasKey(a => a.AddressId);
            builder.Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Province)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.District)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Ward)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Street)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false);
            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
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
                .HasDefaultValue (false);
            builder.Property(p => p.IsPrimary)
                .HasDefaultValue(false);

            builder.HasOne(p => p.User)
                .WithMany(u => u.UserPhones)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);    
        }
        public class UserWalletConfiguration : IEntityTypeConfiguration<UserWallet>
        {
            public void Configure(EntityTypeBuilder<UserWallet> builder)
            {
                builder.ToTable("UserWallets");
                builder.HasKey(u => u.UserWalletId);
                builder.Property(u => u.UserWalletId)
                    .ValueGeneratedOnAdd();

                builder.Property(w => w.WalletAccountRef)
                    .HasMaxLength(255)
                    .IsRequired();
                builder.Property(w => w.WalletType)
                    .HasConversion<int>();
                builder.Property(w => w.IsActive)
                    .HasDefaultValue(false);
                builder.Property(w => w.LinkedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
        public class UserSensitiveConfiguration : IEntityTypeConfiguration<UserSensitve>
        {
            public void Configure(EntityTypeBuilder<UserSensitve> builder)
            {
                builder.ToTable("UserSensitives");
                builder.HasKey(u => u.UserId);

                builder.Property(u => u.CitizenId)
                    .HasMaxLength(20);

                // ValueConverter dung de chuyen doi giua:
                // DateOnly ( Kieu Domain)
                // DateTime (Kieu EF Core, SQL Server dung)
                var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                    // Tham so 1: chen du lieu hoac cap nhat du lieu (Insert/Update):
                    // DateOnly => DateTime
                    // Ex: DateOnly: 2026:03:14 => DateTime: 2026:03:14 00:00:00
                    d => d.ToDateTime(TimeOnly.MinValue),
                    // Tham so 2: Doc du lieu tu database (Select)
                    // DateTime => DateOnly
                    // chuyen lai 2026:03:14 00:00:00 => 2026:03:14
                    d => DateOnly.FromDateTime(d)
                    );

                builder.Property(u => u.IssueDate)
                    .HasConversion(dateOnlyConverter)
                    // chi dinh kieu cot la DATE thay vi DateTime2 (mac dinh)
                    .HasColumnType("DATE");
                builder.Property(u => u.PlaceOfIssue)
                    .HasMaxLength(100);
            }
        }
        public class OAuthAccountConfiguration : IEntityTypeConfiguration<OAuthAccount>
        {
            public void Configure(EntityTypeBuilder<OAuthAccount> builder)
            {
                builder.ToTable("OAuthAccounts");
                builder.HasKey(o => o.UserId);

                builder.Property(o => o.ProviderUserId)
                    .HasMaxLength(255)
                    .IsRequired();
                builder.Property(o => o.AuthProvider)
                    .HasConversion<int>();
            }
        }
    }
}
