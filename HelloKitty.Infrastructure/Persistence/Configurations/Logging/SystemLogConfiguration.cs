using HelloKitty.Domain.Logging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Logging
{
    public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("SystemLogs");
            builder.HasKey(l => l.LogId);

            builder.Property(l => l.LogLevel)
                .HasConversion<int>();
            builder.Property(l => l.Source)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(l => l.Message)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(l => l.RequestPath)
                .HasMaxLength(255);
            builder.Property(l => l.RequestMethod)
                .HasMaxLength(10);
            builder.Property(l => l.IpAddress)
                .HasMaxLength(45);
            builder.Property(l => l.MetaData)
                .HasColumnType("nvarchar(max)");
            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
