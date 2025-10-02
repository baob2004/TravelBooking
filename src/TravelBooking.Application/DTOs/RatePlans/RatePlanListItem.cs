namespace TravelBooking.Application.DTOs.RatePlans
{
    public class RatePlanListItem
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool BreakfastIncluded { get; set; }
        public bool IsRefundable { get; set; }
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = "VND";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}