using Domain.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Application.Interfaces;

namespace Infrastructure.Security.TokenGenerator
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;

        public AccessTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string CreateToken(User user, IList<Role>? roles)
        {
            var claims = new List<Claim>
            {
                new("id", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, user.Username.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.ToString())
            };

            roles?.ToList().ForEach(role => claims.Add(new(ClaimTypes.Role, role.Name)));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}