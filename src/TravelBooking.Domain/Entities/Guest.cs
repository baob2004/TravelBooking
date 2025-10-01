using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class Guest
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string FullName { get; set; } = default!;
        public int? Age { get; set; }
        public string? Note { get; set; }

        // Nav
        public Booking? Booking { get; set; }
    }
}