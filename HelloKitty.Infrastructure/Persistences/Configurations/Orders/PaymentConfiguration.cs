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
}
