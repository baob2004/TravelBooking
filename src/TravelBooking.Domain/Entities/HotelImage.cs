using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class HotelImage
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string Url { get; set; } = default!;
        public bool IsCover { get; set; }
        public int SortOrder { get; set; }

        // Nav
        public Hotel? Hotel { get; set; }
    }
}