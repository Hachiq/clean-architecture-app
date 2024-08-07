﻿namespace Application.Repositories
{
    public interface IUserRolesRepository
    {
        Task AddUserRoleAsync(Guid userId, Guid roleId);
        Task RemoveUserRoleAsync(Guid userId, Guid roleId);
    }
}