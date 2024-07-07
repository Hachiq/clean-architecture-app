using Domain.Entities;

namespace Application.Repositories
{
    public interface ICategoriesRepository
    {
        Task<IList<Category>> GetAllAsync();
    }
}