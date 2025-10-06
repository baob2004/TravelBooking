using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Amenity
{
    public class SetRoomTypeAmenitiesRequest
    {
        public List<Guid> AmenityIds { get; set; } = new();
    }
}