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

        public async Task<IList<SubCategory>> GetAllAsync()
        {
            return await _db.SubCategories.ToListAsync();
        }
    }
}