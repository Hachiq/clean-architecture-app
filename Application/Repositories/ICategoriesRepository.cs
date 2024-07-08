using Domain.Entities;

namespace Application.Repositories
{
    public interface ICategoriesRepository
    {
        Task<Category> GetByIdAsync(Guid id);
        Task<IList<Category>> GetAllAsync();
        Task<IList<Category>> GetBySectionIdAsync(Guid id);
    }
}