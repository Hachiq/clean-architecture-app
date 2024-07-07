namespace Application.Interfaces
{
    public interface IUserRoleService
    {
        Task AssignToUserRole(Guid userId);
        Task AssignToConfirmedUserRole(Guid userId);
        Task AssignToAdminRole(Guid userId);

    }
}