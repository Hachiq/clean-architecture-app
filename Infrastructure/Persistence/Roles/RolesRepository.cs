using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Roles
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

        public async Task<Guid> GetRoleIdByNameAsync(string name)
        {
            var role = await _db.Roles.SingleOrDefaultAsync(role => role.Name == name);
            if (role is null)
            {
                return Guid.Empty;
            }
            return role.Id;
        }
    }
}