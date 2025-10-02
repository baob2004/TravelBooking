using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Hotels
{
    public class SetCoverRequest
    {
        public required Guid ImageId { get; init; }
    }
}