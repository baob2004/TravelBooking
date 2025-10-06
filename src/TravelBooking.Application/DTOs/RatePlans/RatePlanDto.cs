using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.RatePlans
{
    public class RatePlanDto
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; } = default!;
        public bool BreakfastIncluded { get; set; }
        public bool IsRefundable { get; set; }
        public int? MinStayNights { get; set; }
        public int? MaxStayNights { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = "VND";
    }
}