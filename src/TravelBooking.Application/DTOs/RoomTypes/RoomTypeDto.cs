using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.RoomTypes
{
    public class RoomTypeDto
    {
        [Required] public Guid Id { get; set; }
        [Required] public Guid HotelId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, 10)] public int MaxAdults { get; set; }
        [Range(0, 10)] public int MaxChildren { get; set; }

        [Range(0, 1000)]
        public double? SizeSqm { get; set; }

        [Range(0, 5)]
        public decimal AverageRating { get; set; }

        [Range(0, int.MaxValue)]
        public int RatingCount { get; set; }

    }
}