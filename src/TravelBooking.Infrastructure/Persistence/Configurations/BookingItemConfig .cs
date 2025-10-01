using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelBooking.Domain.Entities;

namespace TravelBooking.Infrastructure.Persistence.Configurations
{
    public sealed class BookingItemConfig : IEntityTypeConfiguration<BookingItem>
    {
        public void Configure(EntityTypeBuilder<BookingItem> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.UnitPricePerNight).HasColumnType("decimal(18,2)");
            b.Property(x => x.LineTotal).HasColumnType("decimal(18,2)");

            // Booking -> BookingItems: CHO PHÉP CASCADE (xóa booking thì xóa items của chính booking đó)
            b.HasOne(x => x.Booking)
             .WithMany(bk => bk.Items)
             .HasForeignKey(x => x.BookingId)
             .OnDelete(DeleteBehavior.Cascade);

            // RoomType -> BookingItem: KHÔNG CASCADE (lịch sử vẫn giữ được)
            b.HasOne(x => x.RoomType)
             .WithMany()                            // không cần nav ngược
             .HasForeignKey(x => x.RoomTypeId)
             .OnDelete(DeleteBehavior.Restrict);    // hoặc .NoAction()

            // RatePlan -> BookingItem: KHÔNG CASCADE
            b.HasOne(x => x.RatePlan)
             .WithMany()                            // không cần nav ngược
             .HasForeignKey(x => x.RatePlanId)
             .OnDelete(DeleteBehavior.Restrict);    // hoặc .NoAction()
        }
    }
}