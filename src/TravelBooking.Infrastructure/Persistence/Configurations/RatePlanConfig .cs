using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public sealed class RatePlanConfig : IEntityTypeConfiguration<RatePlan>
    {
        public void Configure(EntityTypeBuilder<RatePlan> b)
        {
            b.HasKey(x => x.Id);

            // FK -> RoomType : cho phép Cascade
            b.HasOne(x => x.RoomType)
             .WithMany(rt => rt.RatePlans)
             .HasForeignKey(x => x.RoomTypeId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK -> Hotel : tránh cascade path
            b.HasOne(x => x.Hotel)
             .WithMany(h => h.RatePlans)
             .HasForeignKey(x => x.HotelId)
             .OnDelete(DeleteBehavior.Restrict); // hoặc .NoAction()
        }
    }
}