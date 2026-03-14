using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Orders.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Orders
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");
            builder.HasKey(s => s.ShipmentId);

            builder.Property(s => s.ShipmentProvider).HasConversion<int>();
            builder.Property(s => s.ShipmentStatus)
                .HasConversion<int>()
                .HasDefaultValue(ShipmentStatus.Pending);

            builder.Property(s => s.TrackingCode).HasMaxLength(100);
        }
    }
}
