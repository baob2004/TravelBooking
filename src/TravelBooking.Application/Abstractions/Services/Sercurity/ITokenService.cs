namespace TravelBooking.Application.Abstractions.Services.Security
{
    public interface ITokenService
    {
        string CreateAccessToken(string userId, string? userName, string? email, IEnumerable<string> roles);
    }
}