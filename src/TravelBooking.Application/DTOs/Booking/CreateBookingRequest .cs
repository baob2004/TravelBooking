using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Booking
{
    public class CreateBookingRequest
    {
        [Required] public Guid CustomerId { get; set; }
        [Required] public Guid HotelId { get; set; }

        [Required] public DateOnly CheckInDate { get; set; }
        [Required] public DateOnly CheckOutDate { get; set; }

        [Required, StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "VND";

        [MinLength(1)]
        public List<BookingItemRequest> Items { get; set; } = new();

        public string? Note { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (CheckOutDate <= CheckInDate)
                yield return new ValidationResult("CheckOutDate phải lớn hơn CheckInDate",
                    new[] { nameof(CheckOutDate) });

            var nights = CheckOutDate.DayNumber - CheckInDate.DayNumber;
            if (nights > 60)
                yield return new ValidationResult("Số đêm không được vượt quá 60.", new[] { nameof(CheckOutDate) });
        }
    }
}