using Microsoft.AspNetCore.Mvc;
using TravelBooking.Application.Abstractions.Services.Identity;
using TravelBooking.Application.DTOs.Auth;

namespace TravelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public Task<AuthResponse> Register([FromBody] RegisterRequest req, CancellationToken ct)
        => auth.RegisterAsync(req, ct);

    [HttpPost("login")]
    public Task<AuthResponse> Login([FromBody] LoginRequest req, CancellationToken ct)
        => auth.LoginAsync(req, ct);
}
