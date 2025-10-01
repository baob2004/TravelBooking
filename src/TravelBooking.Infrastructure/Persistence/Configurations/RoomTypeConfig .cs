using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public sealed class RoomTypeConfig : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> b)
        {
            b.HasKey(x => x.Id);

            b.HasOne(x => x.Hotel)
             .WithMany(h => h.RoomTypes)
             .HasForeignKey(x => x.HotelId)
             .OnDelete(DeleteBehavior.Cascade); // ok
        }
    }
}