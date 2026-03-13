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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(i => i.OrderItemId);

            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(i => i.ProductVariant)
                .WithMany(v => v.OrderItems)
                .HasForeignKey(i => i.VariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
