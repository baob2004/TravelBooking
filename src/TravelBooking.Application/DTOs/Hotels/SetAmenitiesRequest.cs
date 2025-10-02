using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class SetAmenitiesRequest
    {
        [MinLength(0)] public required List<Guid> AmenityIds { get; init; }
    }
}