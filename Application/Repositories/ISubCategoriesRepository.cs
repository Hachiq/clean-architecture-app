using Domain.Entities;

namespace Application.Repositories
{
    public interface ISubCategoriesRepository
    {
        Task<SubCategory> GetByIdAsync(Guid id);
        Task<IList<SubCategory>> GetAllAsync();
        Task<IList<SubCategory>> GetByCategoryId(Guid id);
    }
}