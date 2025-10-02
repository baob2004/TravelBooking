using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Infrastructure.Identity;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            b.Property(x => x.Token).HasMaxLength(512).IsRequired();

            b.HasIndex(x => x.Token).IsUnique();

            b.HasOne<AppUser>()
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}