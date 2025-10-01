using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomNumber { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        // Nav
        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }

    }
}