using HelloKitty.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistence.Configurations.Users
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");
            builder.HasKey(a => a.AddressId);
            builder.Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Province)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.District)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Ward)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Street)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false);
            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
