using HelloKitty.Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Catalog
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");
            builder.HasKey(v => v.VariantId);

            builder.Property(v => v.SKU).HasMaxLength(100);
            builder.HasIndex(v => v.SKU).IsUnique().HasFilter("[SKU] IS NOT NULL");

            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(v => v.Quantity).HasDefaultValue(0);
            builder.Property(v => v.IsActive).HasDefaultValue(false);

            builder.HasMany(v => v.VariantAttributes)
                .WithOne(va => va.ProductVariant)
                .HasForeignKey(va => va.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(v => v.InventoryLogs)
                .WithOne(l => l.ProductVariant)
                .HasForeignKey(l => l.VariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
