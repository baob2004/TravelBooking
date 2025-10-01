using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? BookingId { get; set; }
        public int Score { get; set; }              // 1..5
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Nav
        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }
        public Customer? Customer { get; set; }
        public Booking? Booking { get; set; }
    }
}