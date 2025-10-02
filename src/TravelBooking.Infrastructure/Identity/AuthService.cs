using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TravelBooking.Application.Abstractions.Services.Identity;
using TravelBooking.Application.Abstractions.Services.Security;
using TravelBooking.Application.DTOs.Auth;

namespace TravelBooking.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
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

            return new AuthResponse
            {
                AccessToken = access,
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
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

            var roles = await _userManager.GetRolesAsync(user);
            var access = _tokenService.CreateAccessToken(user.Id, user.UserName, user.Email, roles);

            return new AuthResponse
            {
                AccessToken = access,
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            };
        }
    }
}