namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StarRating { get; set; }
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }
        public string City { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
    }
}