using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Auth
{
    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = "";
    }
}