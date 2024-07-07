namespace Application.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddUserRoleAsync(Guid userId, Guid roleId);
    }
}