using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users
                .Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task UpdateUserRefreshToken(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
