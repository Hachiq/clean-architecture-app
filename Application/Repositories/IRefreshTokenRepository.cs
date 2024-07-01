using Domain.Entities;

namespace Application.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string? token);
        Task ExpireRefreshToken(RefreshToken refreshToken);
        Task UpdateRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
    }
}