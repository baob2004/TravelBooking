using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Inventory
{
    public class BulkUpsertInventoryRequest
    {
        [Required] public Guid HotelId { get; set; }
        [Required] public Guid RoomTypeId { get; set; }
        [Required] public DateOnly From { get; set; }
        [Required] public DateOnly To { get; set; }
        [Range(0, 10000)] public int Allotment { get; set; }
        public List<DayOfWeek>? DaysOfWeek { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (To < From)
                yield return new ValidationResult("To phải ≥ From", new[] { nameof(To) });
        }

    }
}