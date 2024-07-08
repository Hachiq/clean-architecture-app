using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.SubCategories
{
    public class SubCategoriesRepository : ISubCategoriesRepository
    {
        private readonly AppDbContext _db;

        public SubCategoriesRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<SubCategory> GetByIdAsync(Guid id)
        {
            return await _db.SubCategories.FindAsync(id);
        }

        public async Task<IList<SubCategory>> GetAllAsync()
        {
            return await _db.SubCategories.ToListAsync();
        }

        public async Task<IList<SubCategory>> GetByCategoryId(Guid id)
        {
            return await _db.SubCategories.Where(sc => sc.Id == id).ToListAsync();
        }
    }
}