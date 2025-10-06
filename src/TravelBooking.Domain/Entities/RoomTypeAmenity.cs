using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class RoomTypeAmenity
    {
        public Guid RoomTypeId { get; set; }
        public Guid AmenityId { get; set; }

        public RoomType? RoomType { get; set; }
        public Amenity? Amenity { get; set; }
    }
}