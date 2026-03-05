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
}
