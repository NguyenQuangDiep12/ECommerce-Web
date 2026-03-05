using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Attribute = HelloKitty.Domain.Catalog.Entities.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloKitty.Domain.Catalog.Entities;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Catalog
{
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
}
