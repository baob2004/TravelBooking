using System.ComponentModel.DataAnnotations;
using TravelBooking.Application.DTOs.Common;

namespace TravelBooking.Application.DTOs.Search
{
    public class SearchRequest : PagedQuery
    {
        [Required, StringLength(200)]
        public string CityOrKeyword { get; set; } = string.Empty;

        [Required] public DateOnly CheckIn { get; set; }
        [Required] public DateOnly CheckOut { get; set; }

        [Range(1, 20)] public int Adults { get; set; } = 1;
        [Range(0, 20)] public int Children { get; set; } = 0;
        [Range(1, 10)] public int Rooms { get; set; } = 1;

        // (tuỳ chọn) các filter phụ
        [Range(0, 5)] public int? MinStar { get; set; }
        [Range(0, 5)] public int? MaxStar { get; set; }
        public bool? BreakfastIncluded { get; set; }
        public bool? IsRefundable { get; set; }
    }
}