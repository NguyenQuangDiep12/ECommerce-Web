using HelloKitty.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Carts
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(i => i.CartItemId);

            // Không được thêm trùng variant trong cùng 1 cart
            builder.HasIndex(i => new { i.CartId, i.VariantId }).IsUnique();

            builder.Property(i => i.Quantity).IsRequired();

            builder.HasOne(i => i.ProductVariant)
                .WithMany(v => v.CartItems)
                .HasForeignKey(i => i.VariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
