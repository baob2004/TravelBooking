using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Domain.Entities
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public double? SizeSqm { get; set; }
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Nav
        public Hotel? Hotel { get; set; }
        public ICollection<RatePlan> RatePlans { get; set; } = new List<RatePlan>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}