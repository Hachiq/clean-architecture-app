using Domain.Entities;

namespace Application.Repositories
{
    public interface IAdsRepository
    {
        Task<Ad> GetByIdAsync(Guid id);
        Task<IList<Ad>> GetAllAsync();
        Task<IList<Ad>> GetByUserIdAsync(Guid id);
        Task<IList<Ad>> GetBySubCategoryIdAsync(Guid id);
    }
}