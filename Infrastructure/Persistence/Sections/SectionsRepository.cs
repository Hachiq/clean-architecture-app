using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Sections
{
    public class SectionsRepository : ISectionsRepository
    {
        private readonly AppDbContext _db;

        public SectionsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IList<Section>> GetAllAsync()
        {
            return await _db.Sections.ToListAsync();
        }
    }
}