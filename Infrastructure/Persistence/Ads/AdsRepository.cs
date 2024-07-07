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

        public async Task<IList<Ad>> GetAllAsync()
        {
            return await _db.Ads.ToListAsync();
        }
    }
}