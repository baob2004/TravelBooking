using TravelBooking.Domain.ValueObjects;

namespace TravelBooking.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = "";
        public Address Address { get; set; } = default!;
        public int StarRating { get; set; }
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        // Nav
        public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
        public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
        public ICollection<RatePlan> RatePlans { get; set; } = new List<RatePlan>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}