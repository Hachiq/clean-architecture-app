using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Persistence.UserRoles
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly AppDbContext _db;

        public UserRolesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddUserRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };
            await _db.AddAsync(userRole);
            await _db.SaveChangesAsync();
        }
    }
}