using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Application.Abstractions.Services.Identity;
using TravelBooking.Application.Abstractions.Services.Security;
using TravelBooking.Application.DTOs.Auth;
using TravelBooking.Infrastructure.Persistence;

namespace TravelBooking.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            AppDbContext db,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        private static string NewRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private static DateTime RefreshExpiry(int days = 30)
            => DateTime.UtcNow.AddDays(days);

        public async Task<(string UserId, string UserName, string Email, IEnumerable<string> Roles)?> GetCurrentAsync(ClaimsPrincipal principal, CancellationToken ct = default)
        {
            var id = _userManager.GetUserId(principal);
            if (string.IsNullOrEmpty(id)) return null;

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.UserName ?? "", user.Email ?? "", roles);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
        {
            var user = await _userManager.FindByNameAsync(req.UserNameOrEmail)
            ?? await _userManager.FindByEmailAsync(req.UserNameOrEmail);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var pass = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
            if (!pass.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var access = _tokenService.CreateAccessToken(user.Id, user.UserName, user.Email, roles);

            var raw = NewRefreshToken();
            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = raw,
                ExpiresAtUtc = RefreshExpiry(30)
            });
            await _db.SaveChangesAsync(ct);

            return new AuthResponse
            {
                AccessToken = access,
                RefreshToken = raw,
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };
        }

        public async Task<AuthResponse> RefreshAsync(RefreshRequest req, CancellationToken ct = default)
        {
            var token = await _db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == req.RefreshToken, ct);

            if (token is null || token.ExpiresAtUtc <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = await _userManager.FindByIdAsync(token.UserId)
                       ?? throw new UnauthorizedAccessException("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var access = _tokenService.CreateAccessToken(user.Id, user.UserName, user.Email, roles);

            return new AuthResponse
            {
                AccessToken = access,
                RefreshToken = req.RefreshToken,
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = req.UserName,
                Email = req.Email,
                FullName = req.FullName
            };

            var result = await _userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
            {
                var error = string.Join("; ", result.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException(error);
            }

            const string defaultRole = "User";
            await _userManager.AddToRoleAsync(user, defaultRole);

            var roles = await _userManager.GetRolesAsync(user);
            var access = _tokenService.CreateAccessToken(user.Id, user.UserName, user.Email, roles);

            var raw = NewRefreshToken();
            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = raw,
                ExpiresAtUtc = RefreshExpiry(30)
            });
            await _db.SaveChangesAsync(ct);

            return new AuthResponse
            {
                AccessToken = access,
                RefreshToken = raw,
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };
        }
    }
}