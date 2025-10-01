using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class BookingItem
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid RatePlanId { get; set; }
        public int Rooms { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal UnitPricePerNight { get; set; }
        public decimal LineTotal { get; set; }

        // Nav
        public Booking? Booking { get; set; }
        public RoomType? RoomType { get; set; }
        public RatePlan? RatePlan { get; set; }
    }
}