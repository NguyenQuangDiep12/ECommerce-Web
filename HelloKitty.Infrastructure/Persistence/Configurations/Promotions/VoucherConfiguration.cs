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
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");
            builder.HasKey(v => v.VoucherId);

            builder.Property(v => v.Code).HasMaxLength(50).IsRequired();
            builder.HasIndex(v => v.Code).IsUnique();

            builder.Property(v => v.DiscountType).HasConversion<int>();

            builder.Property(v => v.DiscountValue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(v => v.MinOrderAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(v => v.MaxDiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.IsActive).HasDefaultValue(false);

            builder.HasMany<VoucherUsage>()
                .WithOne(vu => vu.Voucher)
                .HasForeignKey(vu => vu.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
