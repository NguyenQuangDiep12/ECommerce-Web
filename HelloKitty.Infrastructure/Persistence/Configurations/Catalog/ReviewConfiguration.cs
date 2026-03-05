using HelloKitty.Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Catalog
{
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
}
