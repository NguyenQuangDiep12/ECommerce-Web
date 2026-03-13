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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderStatus)
                .HasConversion<int>()
                .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.FinalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Shipment)
                .WithOne(s => s.Order)
                .HasForeignKey<Shipment>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Address)
                .WithOne(a => a.Order)
                .HasForeignKey<OrderAddress>(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
