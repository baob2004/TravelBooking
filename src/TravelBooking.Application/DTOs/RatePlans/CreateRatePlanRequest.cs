using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.RatePlans
{
    public class CreateRatePlanRequest : IValidatableObject
    {
        [Required] public Guid HotelId { get; set; }
        [Required] public Guid RoomTypeId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool BreakfastIncluded { get; set; }
        public bool IsRefundable { get; set; }

        [Range(1, 365)] public int? MinStayNights { get; set; }
        [Range(1, 365)] public int? MaxStayNights { get; set; }

        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }

        [Range(0, 1_000_000)]
        public decimal PricePerNight { get; set; }

        [Required, StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "VND";

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (EndDate < StartDate)
                yield return new ValidationResult("EndDate phải ≥ StartDate", new[] { nameof(EndDate) });

            if (MinStayNights.HasValue && MaxStayNights.HasValue && MinStayNights > MaxStayNights)
                yield return new ValidationResult("MinStayNights phải ≤ MaxStayNights",
                    new[] { nameof(MinStayNights), nameof(MaxStayNights) });
        }
    }
}