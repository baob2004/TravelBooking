using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!; // unique (case-insensitive) tuỳ bạn
        public string? Icon { get; set; }            // tuỳ chọn: "shower", "wind", ...
        public ICollection<RoomTypeAmenity> RoomTypeAmenities { get; set; } = new List<RoomTypeAmenity>();
    }
}