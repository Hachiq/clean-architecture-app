using Domain.Entities;

namespace Application.Repositories
{
    public interface IAdsRepository
    {
        Task<IList<Ad>> GetAllAsync();
    }
}