using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Ratings
{
    public class CreateRatingRequest
    {
        [Required] public Guid HotelId { get; set; }
        public Guid? RoomTypeId { get; set; }
        [Required] public Guid CustomerId { get; set; }

        [Range(1, 5)] public int Score { get; set; }

        [StringLength(120)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }

    }
}