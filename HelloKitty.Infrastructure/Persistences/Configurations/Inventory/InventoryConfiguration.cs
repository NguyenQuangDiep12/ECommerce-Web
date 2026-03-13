using HelloKitty.Domain.Inventory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Inventory
{
    public class InventoryLogConfiguration : IEntityTypeConfiguration<InventoryLog>
    {
        public void Configure(EntityTypeBuilder<InventoryLog> builder)
        {
            builder.ToTable("InventoryLogs");
            builder.HasKey(l => l.LogId);

            builder.Property(l => l.ChangeType)
                .HasConversion<int>();
            builder.Property(l => l.QuantityChange)
                .IsRequired();
            builder.Property(l => l.CurrentStock)
                .IsRequired();
            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
