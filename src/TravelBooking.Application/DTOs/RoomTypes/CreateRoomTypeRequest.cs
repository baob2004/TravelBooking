namespace TravelBooking.Application.DTOs.RoomTypes
{
    public class CreateRoomTypeRequest
    {
        public Guid HotelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public double? SizeSqm { get; set; }
    }
}