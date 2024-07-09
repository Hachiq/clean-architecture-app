namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserByIdAsync(Guid id);
    }
}