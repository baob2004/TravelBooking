namespace TravelBooking.Application.DTOs.Booking
{
    public class QuoteResponse
    {
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "VND";
        public List<BookingItemResponse> Items { get; set; } = new();
    }
}