using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelBooking.Application.Abstractions.Services.Security;

namespace TravelBooking.Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _opt;
        private readonly SigningCredentials _creds;

        public TokenService(IOptions<JwtOptions> opt)
        {
            _opt = opt.Value;
            _creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key)),
                SecurityAlgorithms.HmacSha256);
        }

        public string CreateAccessToken(string userId, string? userName, string? email, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Email, email ?? string.Empty),
                new(ClaimTypes.Name, userName ?? email ?? userId)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var jwt = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opt.ExpiresMinutes),
                signingCredentials: _creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}