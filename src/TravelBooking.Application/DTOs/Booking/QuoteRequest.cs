namespace TravelBooking.Application.DTOs.Booking
{
    public class QuoteRequest
    {
        public Guid HotelId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public List<BookingItemRequest> Items { get; set; } = new();
    }
}