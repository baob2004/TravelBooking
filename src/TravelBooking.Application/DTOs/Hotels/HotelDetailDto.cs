using TravelBooking.Application.DTOs.RatePlans;
using TravelBooking.Application.DTOs.Ratings;
using TravelBooking.Application.DTOs.RoomTypes;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelDetailDto
    {
        public string Description { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string State { get; set; } = string.Empty;
        public string City { get; private set; } = default!;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public List<HotelImageDto> Images { get; set; } = new();
        public List<RoomTypeDto> RoomTypes { get; set; } = new();
        public List<RatePlanDto> RatePlans { get; set; } = new();
        public List<RatingDto> Ratings { get; set; } = new();
    }
}