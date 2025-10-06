using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBooking.Application.DTOs.Amenity
{
    public class CreateAmenityRequest : IValidatableObject
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Icon { get; set; }  // tuỳ chọn

        public IEnumerable<ValidationResult> Validate(ValidationContext _)
        {
            if (string.IsNullOrWhiteSpace(Name))
                yield return new ValidationResult("Name is required.", [nameof(Name)]);
        }
    }
}