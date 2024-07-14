using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _db;
        public UsersRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _db.Users
                .Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _db.Users
                .Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByRefreshTokenIdAsync(Guid id)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.RefreshTokenId == id);
        }

        public async Task ConfirmEmailAsync(User user)
        {
            user.EmailConfirmed = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
