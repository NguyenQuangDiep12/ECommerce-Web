using HelloKitty.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Orders
{
    public class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    {
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.ToTable("OrderAddresses");
            builder.HasKey(a => a.OrderId);

            builder.Property(a => a.ReceiverName).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Phone).HasMaxLength(15).IsRequired();
            builder.Property(a => a.Province).HasMaxLength(100).IsRequired();
            builder.Property(a => a.District).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Ward).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Street).HasMaxLength(255).IsRequired();
        }
    }
}
