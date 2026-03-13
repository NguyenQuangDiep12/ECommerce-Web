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
}
