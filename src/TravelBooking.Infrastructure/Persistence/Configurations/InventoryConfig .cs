using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public sealed class InventoryConfig : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Date).HasColumnType("date");

            b.HasOne(x => x.Hotel)
             .WithMany(h => h.Inventories)
             .HasForeignKey(x => x.HotelId)
             .OnDelete(DeleteBehavior.Restrict); // hoặc .NoAction()

            b.HasOne(x => x.RoomType)
             .WithMany(rt => rt.Inventories)
             .HasForeignKey(x => x.RoomTypeId)
             .OnDelete(DeleteBehavior.Restrict); // hoặc .NoAction()
        }
    }
}