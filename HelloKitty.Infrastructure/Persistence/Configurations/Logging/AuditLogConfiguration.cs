using HelloKitty.API.Domain.Logging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Logging
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(l => l.AuditLogId);

            builder.Property(l => l.AuditAction).HasConversion<int>();
            builder.Property(l => l.TableName).HasMaxLength(100).IsRequired();
            builder.Property(l => l.RecordId).HasMaxLength(100).IsRequired();
            builder.Property(l => l.UserEmail).HasMaxLength(150);
            builder.Property(l => l.IpAddress).HasMaxLength(45);
            builder.Property(l => l.UserAgent).HasMaxLength(500);
            builder.Property(l => l.EndPoint).HasMaxLength(255);

            // OldValues/NewValues là JSON, có thể dài
            builder.Property(l => l.OldValue).HasColumnType("nvarchar(max)");
            builder.Property(l => l.NewValue).HasColumnType("nvarchar(max)");

            builder.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Không có FK sang Users — loose reference
            builder.HasIndex(l => l.UserId);
            builder.HasIndex(l => l.CreatedAt);
        }
    }
}
