using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Booking
{
    public class BookingItemRequest
    {
        [Required] public Guid RoomTypeId { get; set; }
        [Required] public Guid RatePlanId { get; set; }

        [Range(1, 50)] public int Rooms { get; set; }

        [Range(1, 20)] public int Adults { get; set; }
        [Range(0, 20)] public int Children { get; set; }
    }
}