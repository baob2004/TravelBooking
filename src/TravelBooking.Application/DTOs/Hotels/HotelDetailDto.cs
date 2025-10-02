using TravelBooking.Application.DTOs.RoomTypes;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelDetailDto
    {
        public string Description { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public List<HotelImageDto> Images { get; set; } = new();
        public List<RoomTypeDto> RoomTypes { get; set; } = new();
    }
}