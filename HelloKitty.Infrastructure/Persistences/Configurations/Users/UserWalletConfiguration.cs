using HelloKitty.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Persistences.Configurations.Users
{
    public class UserWalletConfiguration : IEntityTypeConfiguration<UserWallet>
    {
        public void Configure(EntityTypeBuilder<UserWallet> builder)
        {
            builder.ToTable("UserWallets");
            builder.HasKey(u => u.UserWalletId);
            builder.Property(u => u.UserWalletId)
                .ValueGeneratedOnAdd();

            builder.Property(w => w.WalletAccountRef)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(w => w.WalletType)
                .HasConversion<int>();
            builder.Property(w => w.IsActive)
                .HasDefaultValue(false);
            builder.Property(w => w.LinkedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
