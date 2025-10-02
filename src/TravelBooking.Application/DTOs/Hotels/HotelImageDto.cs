using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelImageDto
    {
        [Required] public Guid Id { get; set; }
        [Required] public Guid HotelId { get; set; }

        [Required, Url, StringLength(1000)]
        public string Url { get; set; } = string.Empty;

        public bool IsCover { get; set; }
        [Range(0, 9999)] public int SortOrder { get; set; }
    }
}