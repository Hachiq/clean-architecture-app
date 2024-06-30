using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateToken();
    }
}