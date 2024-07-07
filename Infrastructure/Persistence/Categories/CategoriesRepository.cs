using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly AppDbContext _db;

        public CategoriesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }
    }
}