using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public class RoomTypeAmenityConfig : IEntityTypeConfiguration<RoomTypeAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeAmenity> b)
        {
            b.HasKey(x => new { x.RoomTypeId, x.AmenityId });

            b.HasOne(x => x.RoomType)
             .WithMany(rt => rt.RoomTypeAmenities)
             .HasForeignKey(x => x.RoomTypeId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Amenity)
             .WithMany(a => a.RoomTypeAmenities)
             .HasForeignKey(x => x.AmenityId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}