namespace TravelBooking.Application.DTOs.Booking
{
    public class BookingFilter
    {
        public Guid? CustomerId { get; set; }
        public Guid? HotelId { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
        public string? Status { get; set; } // Pending/Confirmed/...
    }
}