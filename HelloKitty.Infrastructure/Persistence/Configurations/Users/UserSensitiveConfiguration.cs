using HelloKitty.Domain.Users.Entities;
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
}
