using Application.Interfaces;
using Domain.Entities;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public RefreshTokenService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public RefreshToken CreateToken()
        {
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = _dateTimeProvider.UtcNow,
                ExpiresAt = _dateTimeProvider.UtcNow.AddDays(30)
            };

            return refreshToken;
        }
    }
}