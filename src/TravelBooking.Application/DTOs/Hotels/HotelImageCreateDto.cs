using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class HotelImageCreateDto
    {
        public required string Url { get; init; }
        public bool IsCover { get; init; } = false;
        public string? Caption { get; init; }
        public int? SortOrder { get; set; }
    }
}