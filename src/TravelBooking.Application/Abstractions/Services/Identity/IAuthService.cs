using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelBooking.Application.DTOs.Auth;

namespace TravelBooking.Application.Abstractions.Services.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
        Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default);
        Task<(string UserId, string UserName, string Email, IEnumerable<string> Roles)?> GetCurrentAsync(ClaimsPrincipal principal, CancellationToken ct = default);
        Task<AuthResponse> RefreshAsync(RefreshRequest req, CancellationToken ct = default);
    }
}