using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Application.DTOs.Auth
{
    public class LoginRequest
    {
        [Required, StringLength(320)]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}