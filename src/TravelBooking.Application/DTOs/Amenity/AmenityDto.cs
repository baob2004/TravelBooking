using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Amenity
{
    public class AmenityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
    }
}