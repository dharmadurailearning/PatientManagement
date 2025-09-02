using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PatientManagement.Api.Auth
{
    public class JwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly SymmetricSecurityKey _key;
        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        }
        public string GenerateToken(string username, IEnumerable<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (roles != null)
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}