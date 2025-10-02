using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class AddImagesRequest
    {
        [MinLength(1)] public required List<HotelImageCreateDto> Images { get; init; }
    }
}