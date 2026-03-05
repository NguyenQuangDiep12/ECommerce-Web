using HelloKitty.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Promotions
{
    public class VoucherUsageConfiguration : IEntityTypeConfiguration<VoucherUsage>
    {
        public void Configure(EntityTypeBuilder<VoucherUsage> builder)
        {
            builder.ToTable("VoucherUsages");
            // Composite PK: 1 voucher chỉ dùng 1 lần trên 1 order
            builder.HasKey(vu => new { vu.VoucherId, vu.OrderId });

            builder.Property(vu => vu.UsedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(vu => vu.Order)
                .WithOne(o => o.VoucherUsage)
                .HasForeignKey<VoucherUsage>(vu => vu.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
