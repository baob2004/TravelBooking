using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.RoomTypes
{
    public class CreateRoomTypeRequest : IValidatableObject
    {

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, 20)] public int MaxAdults { get; set; }
        [Range(0, 20)] public int MaxChildren { get; set; }

        [Range(0.0, 10_000.0)]
        public double? SizeSqm { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("Name là bắt buộc.", new[] { nameof(Name) });

            if (SizeSqm.HasValue && SizeSqm.Value <= 0)
                yield return new ValidationResult("SizeSqm phải > 0.", new[] { nameof(SizeSqm) });
        }
    }
}