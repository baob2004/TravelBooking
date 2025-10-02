namespace TravelBooking.Application.DTOs.Booking
{
    public class BookingItemResponse
    {
        public Guid RoomTypeId { get; set; }
        public Guid RatePlanId { get; set; }

        public int Rooms { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }

        public decimal UnitPricePerNight { get; set; }
        public decimal LineTotal { get; set; }

        // (tuỳ chọn) hiển thị
        public string? RoomTypeName { get; set; }
        public string? RatePlanName { get; set; }
    }
}