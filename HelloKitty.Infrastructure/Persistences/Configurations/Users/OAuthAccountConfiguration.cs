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
    public class OAuthAccountConfiguration : IEntityTypeConfiguration<OAuthAccount>
    {
        public void Configure(EntityTypeBuilder<OAuthAccount> builder)
        {
            builder.ToTable("OAuthAccounts");
            builder.HasKey(o => o.OAuthAccountId);

            builder.Property(o => o.ProviderUserId)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(o => o.AuthProvider)
                .HasConversion<int>();
        }
    }
}
