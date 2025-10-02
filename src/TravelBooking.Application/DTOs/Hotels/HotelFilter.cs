namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelFilter
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Keyword { get; set; }     // tìm theo tên/mô tả
        public int? MinStar { get; set; }
        public int? MaxStar { get; set; }
        public decimal? MinAverageRating { get; set; } // 0..5
    }
}