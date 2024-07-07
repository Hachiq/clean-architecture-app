using Domain.Entities;

namespace Application.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string? token);
        Task ExpireRefreshToken(RefreshToken refreshToken);
        Task UpdateRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
    }
}