using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Ads
{
    public class AdsRepository : IAdsRepository
    {
        private readonly AppDbContext _db;

        public AdsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Ad> GetByIdAsync(Guid id)
        {
            return await _db.Ads.FindAsync(id);
        }

        public async Task<IList<Ad>> GetAllAsync()
        {
            return await _db.Ads.ToListAsync();
        }

        public async Task<IList<Ad>> GetByUserIdAsync(Guid id)
        {
            return await _db.Ads.Where(a => a.UserId == id).ToListAsync();
        }

        public async Task<IList<Ad>> GetBySubCategoryIdAsync(Guid id)
        {
            return await _db.Ads.Where(a => a.SubCategoryId == id).ToListAsync();
        }
    }
}