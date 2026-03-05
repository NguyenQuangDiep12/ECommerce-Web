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
