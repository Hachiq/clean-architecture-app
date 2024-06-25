using Domain.Entities;

namespace Application.Services.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateToken();
    }
}