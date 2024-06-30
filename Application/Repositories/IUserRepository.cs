using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task UpdateUserRefreshToken(User user, RefreshToken refreshToken);
    }
}
