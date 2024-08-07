﻿using Domain.Entities;

namespace Application.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByRefreshTokenIdAsync(Guid id);
        Task ConfirmEmailAsync(User user);
        Task UpdateAsync(User user);
    }
}
