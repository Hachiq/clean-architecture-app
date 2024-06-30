﻿using Domain.Entities;

namespace Application.Repositories
{
    public interface IRolesRepository
    {
        Task<IList<Role>?> GetByUserIdAsync(Guid id);
    }
}