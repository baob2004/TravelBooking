using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class RatePlan
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool BreakfastIncluded { get; set; }
        public bool IsRefundable { get; set; }
        public int? MinStayNights { get; set; }
        public int? MaxStayNights { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = "VND";

        // Nav
        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }
    }
}