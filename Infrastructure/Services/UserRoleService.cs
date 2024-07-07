using Application.Interfaces;
using Application.Repositories;

namespace Infrastructure.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IRolesRepository rolesRepository, IUserRoleRepository userRoleRepository)
        {
            _rolesRepository = rolesRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task AssignToAdminRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("Admin");
            await _userRoleRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task AssignToConfirmedUserRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("ConfirmedUser");
            await _userRoleRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task AssignToUserRole(Guid userId)
        {
            var roleId = await _rolesRepository.GetRoleIdByNameAsync("User");
            await _userRoleRepository.AddUserRoleAsync(userId, roleId);
        }
    }
}