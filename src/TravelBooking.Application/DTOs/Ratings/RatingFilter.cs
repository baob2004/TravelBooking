namespace TravelBooking.Application.DTOs.Ratings
{
    public class RatingFilter
    {
        public Guid? HotelId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? MinScore { get; set; }  // 1..5
        public int? MaxScore { get; set; }  // 1..5

    }
}