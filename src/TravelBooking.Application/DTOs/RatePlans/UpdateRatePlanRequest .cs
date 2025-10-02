namespace TravelBooking.Application.DTOs.RatePlans
{
    public class UpdateRatePlanRequest : CreateRatePlanRequest
    {
        public Guid Id { get; set; }
    }
}