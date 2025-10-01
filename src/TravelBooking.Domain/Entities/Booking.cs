using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBooking.Domain.Enums;

namespace TravelBooking.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public Guid CustomerId { get; set; }
        public Guid HotelId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
        public int TotalGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Currency { get; set; } = "VND";
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Nav
        public Customer? Customer { get; set; }
        public Hotel? Hotel { get; set; }
        public ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}