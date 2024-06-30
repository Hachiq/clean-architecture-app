using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenRepository(AppDbContext db, IDateTimeProvider dateTimeProvider)
        {
            _db = db;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string? token)
        {
            return await _db.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task ExpireRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.ExpiresAt = _dateTimeProvider.UtcNow;
            _db.Update(refreshToken);
            await _db.SaveChangesAsync();
        }
    }
}