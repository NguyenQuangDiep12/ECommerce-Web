using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Orders.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Orders
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

    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentMethod).HasConversion<int>();
            builder.Property(p => p.PaymentStatus)
                .HasConversion<int>()
                .HasDefaultValue(PaymentStatus.Pending);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.FailureReason).HasMaxLength(500);
            builder.Property(p => p.GatewayResponse).HasMaxLength(2000);
        }
    }

    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("Refunds");
            builder.HasKey(r => r.RefundId);

            builder.Property(r => r.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(r => r.Reason).HasMaxLength(500);
            builder.Property(r => r.RefundStatus)
                .HasConversion<int>()
                .HasDefaultValue(RefundStatus.Pending);
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.Payment)
                .WithMany()
                .HasForeignKey(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

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
