using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Roles
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext _db;

        public RolesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IList<Role>?> GetByUserIdAsync(Guid id)
        {
            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == id)
                .Select(ur => ur.Role)
                .ToListAsync();

            return roles;
        }
    }
}