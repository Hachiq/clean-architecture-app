using Application.Interfaces;
using Application.Repositories;

namespace Infrastructure.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserRolesRepository _userRolesRepository;

        public UserRoleService(IRolesRepository rolesRepository, IUserRolesRepository userRolesRepository)
        {
            _rolesRepository = rolesRepository;
            _userRolesRepository = userRolesRepository;
        }

        public async Task AssignToAdminRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("Admin");
            await _userRolesRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task AssignToConfirmedUserRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("ConfirmedUser");
            await _userRolesRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task AssignToUserRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("User");
            await _userRolesRepository.AddUserRoleAsync(userId, roleId);
        }
    }
}