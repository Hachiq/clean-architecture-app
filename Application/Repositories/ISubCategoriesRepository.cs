using Domain.Entities;

namespace Application.Repositories
{
    public interface ISubCategoriesRepository
    {
        Task<IList<SubCategory>> GetAllAsync();
    }
}