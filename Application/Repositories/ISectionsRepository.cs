using Domain.Entities;

namespace Application.Repositories
{
    public interface ISectionsRepository
    {
        Task<IList<Section>> GetAllAsync();
    }
}