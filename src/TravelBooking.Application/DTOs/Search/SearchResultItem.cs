namespace TravelBooking.Application.DTOs.Search
{
    public class SearchResultItem
    {
        // Khóa
        public Guid HotelId { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid RatePlanId { get; set; }

        // Hiển thị
        public string HotelName { get; set; } = string.Empty;
        public string RoomTypeName { get; set; } = string.Empty;
        public string RatePlanName { get; set; } = string.Empty;

        // Giá & tồn
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = "VND";
        public int MaxAvailableRooms { get; set; }

        // Đánh giá / hạng sao
        public int StarRating { get; set; }
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }

        // (tuỳ chọn) địa điểm rút gọn
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}