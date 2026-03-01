using HelloKitty.API.Domain.Catalog.Entities;
using HelloKitty.API.Domain.Catalog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Attribute = HelloKitty.API.Domain.Catalog.Entities.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Catalog
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.CategoryName).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Slug).HasMaxLength(150).IsRequired();
            builder.HasIndex(c => c.Slug).IsUnique();
            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Self-referencing (parent-child)
            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductName).HasMaxLength(255).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(4000);
            builder.Property(p => p.ProductStatus)
                .HasConversion<int>()
                .HasDefaultValue(ProductStatus.Draft);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductVariants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductImages)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

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

    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(i => i.ImageId);

            builder.Property(i => i.ImageUrl).HasMaxLength(500).IsRequired();
            builder.Property(i => i.IsPrimary).HasDefaultValue(false);
        }
    }

    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.ReviewId);

            builder.Property(r => r.Rating)
                .HasColumnType("decimal(2,1)")
                .IsRequired();

            // Rating chỉ từ 1.0 đến 5.0
            builder.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1.0 AND [Rating] <= 5.0"));

            builder.Property(r => r.Comment).HasMaxLength(1000);
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Mỗi user chỉ review 1 lần trên 1 sản phẩm
            builder.HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();
        }
    }

    public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
    {
        public void Configure(EntityTypeBuilder<Attribute> builder)
        {
            builder.ToTable("Attributes");
            builder.HasKey(a => a.AttributeId);

            builder.Property(a => a.AttributeName).HasMaxLength(100).IsRequired();
            builder.HasIndex(a => a.AttributeName).IsUnique();

            builder.HasMany(a => a.AttributeValues)
                .WithOne(v => v.Attribute)
                .HasForeignKey(v => v.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
    {
        public void Configure(EntityTypeBuilder<AttributeValue> builder)
        {
            builder.ToTable("AttributeValues");
            builder.HasKey(v => v.ValueId);

            builder.Property(v => v.ValueName).HasMaxLength(100).IsRequired();
        }
    }

    public class VariantAttributeConfiguration : IEntityTypeConfiguration<VariantAttribute>
    {
        public void Configure(EntityTypeBuilder<VariantAttribute> builder)
        {
            builder.ToTable("VariantAttributes");
            // Composite PK
            builder.HasKey(va => new { va.VariantId, va.ValueId });

            builder.HasOne(va => va.AttributeValue)
                .WithMany()
                .HasForeignKey(va => va.ValueId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
